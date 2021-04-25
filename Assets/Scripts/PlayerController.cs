using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public float moveSpeed;

    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private Vector2 startPosition;

    public Collectible collectible;

    private void Start()
    {
        startPosition = transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    void FixedUpdate()
    {
        if (!gameController.active) return;

        var screenCenter = camera.ScreenToWorldPoint(new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            camera.nearClipPlane));

        if (!collectible && rigidbody.velocity.y > 0 && transform.position.y >= screenCenter.y)
        {
            gameController.SetCameraY(transform.position.y);
        }

        float xMotion = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float yMotion = moveSpeed * Time.deltaTime;
        if (Input.GetAxis("Vertical") < 0) yMotion *= -1;

        rigidbody.velocity = new Vector3(xMotion, yMotion);

        if (collectible)
        {
            rigidbody.velocity = new Vector3(xMotion, -1.5f * moveSpeed * Time.deltaTime);
            rigidbody.drag = collectible.weight;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            gameController.Exit();
        } else if (!collectible && other.CompareTag("Collectible"))
        {
            collectible = other.gameObject.GetComponent<Collectible>();
            collectible.Capture(transform);
        }
    }

    public void DropCollectible()
    {
        if (!collectible) return;

        collectible.Release();
        collectible = null;
    }

    public void Reset()
    {
        if (collectible) Destroy(collectible.gameObject);

        transform.position = startPosition;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;
        rigidbody.drag = 0;
    }
}
