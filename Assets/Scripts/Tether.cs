using System;
using UnityEngine;

public class Tether : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    public int resolution = 100;
    public Vector3 offset = new Vector3(0.7f, 0 ,0);

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        var start = player.position + offset;
        var end = exit.position + offset;

        var distance = Vector2.Distance(start, end) / 2;
        var midpoint = distance * Vector3.Normalize(end - start) + start;

        var pivot = new Vector3(end.x, midpoint.y, 0);

        lineRenderer.positionCount = resolution;
        float t = 0f;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            var position = (1 - t) * (1 - t) * start + 2 * (1 - t) * t * pivot + t * t * end;
            lineRenderer.SetPosition(i, position);
            t += 1 / (float)lineRenderer.positionCount;
        }
    }
}