using System;
using System.Collections;
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
    private SoundController soundController;

    void Awake()
    {
        soundController = GetComponent<SoundController>();
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
        soundController.PlayIncreaseOxygen();
        var rectTransform = slider.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width + collectible.value, rectTransform.rect.height);
        maxSupply += collectible.value;
        supply = maxSupply;
        StartCoroutine("FillSupplyBar");
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

    private IEnumerator FillSupplyBar()
    {
        var fill = slider.GetComponentsInChildren<Image>()[1];
        fill.color = Color.green;

        while (slider.value < 1)
        {
            slider.value += Time.deltaTime * 2;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        fill.color = Color.white;
    }
}