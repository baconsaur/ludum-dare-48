using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int weight;
    public int value = 10;
    public int defaultRotationSpeed = 10;
    public int grabSpeed = 10;

    private int rotationSpeed;
    private IEnumerator coroutine;
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
        coroutine = HoldObject(parent);
        StartCoroutine(coroutine);
    }

    public void Release()
    {
        soundController.PlayLoseItem();
        StopCoroutine(coroutine);
        transform.SetParent(null);
        rotationSpeed = defaultRotationSpeed;
    }

    IEnumerator HoldObject(Transform parent)
    {
        var renderer = GetComponent<Renderer>();
        var offset = new Vector3(0, Vector2.Distance(renderer.bounds.min, transform.position), 0);
        var target = parent.position - offset;

        while (transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * grabSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
