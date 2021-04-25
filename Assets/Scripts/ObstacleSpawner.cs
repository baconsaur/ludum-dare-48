using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public float maxRotationSpeed;
    public float maxDriftSpeed;

    private GameObject obj;

    public GameObject Spawn(Vector2 position)
    {
        obj = objects[Random.Range(0, objects.Length - 1)];
        var newObstacle = Instantiate(obj, position, Quaternion.identity);

        var rigidbody = newObstacle.GetComponent<Rigidbody2D>();

        var rigidbodyAngularVelocity = Random.Range(-1f, 1f) * maxRotationSpeed;
        var driftSpeed = Random.Range(-1f, 1f) * maxDriftSpeed;

        rigidbody.angularVelocity = rigidbodyAngularVelocity;
        rigidbody.velocity = new Vector2(driftSpeed, driftSpeed);

        return newObstacle;
    }
}
