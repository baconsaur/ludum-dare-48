using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int weight;
    public int value = 10;
    public int defaultRotationSpeed = 10;

    private int rotationSpeed;

    void Start()
    {
        rotationSpeed = defaultRotationSpeed;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void Capture(Transform parent)
    {
        transform.SetParent(parent);
        rotationSpeed = 0;
    }

    public void Release()
    {
        transform.SetParent(null);
        rotationSpeed = defaultRotationSpeed;
    }
}
