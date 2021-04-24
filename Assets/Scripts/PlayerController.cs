using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public float floatSpeed;
    public float retractSpeed;

    private new Rigidbody2D rigidbody;
    private new Camera camera;

    public Collectible collectible;

    private void Start()
    {
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

        float xMotion = Input.GetAxis("Horizontal") * floatSpeed * Time.deltaTime;
        rigidbody.velocity = new Vector3(xMotion, rigidbody.velocity.y);

        if (collectible)
        {
            rigidbody.gravityScale = retractSpeed / floatSpeed;
            rigidbody.drag = collectible.weight;
            return;
        }

        if (Input.GetAxis("Fire1") == 1)
        {
            rigidbody.gravityScale = retractSpeed / floatSpeed;
        }
        else
        {
            rigidbody.gravityScale = -1;
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

        transform.position = Vector2.zero;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;
        rigidbody.drag = 0;
    }
}
