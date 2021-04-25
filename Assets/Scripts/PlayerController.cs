using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public GameObject exitIndicator;
    public float moveSpeed;
    public float retractionDelaySeconds = 1;

    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private Vector2 startPosition;
    private bool canRetract;

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
        if (Input.GetAxis("Vertical") < 0 && canRetract)
        {
            ShowExit();
            yMotion *= -1;
        }
        else if (!collectible)
        {
            HideExit();
        }

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
            GrabCollectible(other.transform.parent.gameObject);
        }
    }

    private void GrabCollectible(GameObject obj)
    {
        ShowExit();
        collectible = obj.GetComponent<Collectible>();
        collectible.Capture(transform);
    }

    public void DropCollectible()
    {
        if (!collectible) return;

        collectible.Release();
        collectible = null;
    }

    private void ShowExit()
    {
        exitIndicator.SetActive(true);
    }

    private void HideExit()
    {
        exitIndicator.SetActive(false);
    }

    public void Reset()
    {
        HideExit();

        if (collectible) Destroy(collectible.gameObject);

        transform.position = startPosition;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;
        rigidbody.drag = 0;
    }

    public IEnumerator SetRetractionDelay()
    {
        canRetract = false;
        yield return new WaitForSeconds(retractionDelaySeconds);
        canRetract = true;
    }
}
