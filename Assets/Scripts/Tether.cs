using System;
using UnityEngine;

public class Tether : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        var start = player.position;
        var end = exit.position;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}