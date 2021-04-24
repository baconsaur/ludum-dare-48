using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public Slider slider;
    public float maxSupply = 100;
    public float lossRate;
    public bool active;
    public GameObject menu;

    private float supply;

    void Start()
    {
        supply = maxSupply;
    }

    void Update()
    {
        if (active) return;

        if (Input.anyKeyDown)
        {
            Enter();
        }
    }

    void FixedUpdate()
    {
        if (!active) return;

        supply -= lossRate * Time.deltaTime;
        slider.value = supply / maxSupply;

        if (supply <= 0)
        {
            playerController.DropCollectible();
            Exit();
        }
    }

    public void UpgradeSupply(Collectible collectible)
    {
        var rectTransform = slider.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width + collectible.value, rectTransform.rect.height);
        maxSupply += collectible.value;
        supply = maxSupply;
    }

    public void Exit()
    {
        if (playerController.collectible) UpgradeSupply(playerController.collectible);
        playerController.Reset();
        active = false;
        menu.SetActive(true);
    }

    public void Enter()
    {
        menu.SetActive(false);
        SetCameraY(0);
        active = true;
        supply = maxSupply;
    }

    public void SetCameraY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}