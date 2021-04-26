using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int weight;
    public int value = 10;
    public int defaultRotationSpeed = 10;
    public int carryRotation = -90;

    private int rotationSpeed;
    private SoundController soundController;

    void Awake()
    {
        soundController = Camera.main.GetComponent<SoundController>();
        rotationSpeed = defaultRotationSpeed;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void Capture(Transform parent)
    {
        soundController.PlayGrabItem();
        transform.SetParent(parent);
        rotationSpeed = 0;
        HoldObject(parent);
    }

    public void Release()
    {
        soundController.PlayLoseItem();
        transform.SetParent(null);
        rotationSpeed = defaultRotationSpeed;
    }

    void HoldObject(Transform parent)
    {
        var parentRenderer = parent.GetComponent<Renderer>();

        var distanceToPlayerHead = parentRenderer.bounds.max.y - parent.position.y;

        transform.position = parent.position + new Vector3(0, distanceToPlayerHead, 0);
        transform.rotation = Quaternion.Euler(0, 0, carryRotation);
    }
}
