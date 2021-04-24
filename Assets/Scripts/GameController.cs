using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public Slider slider;
    public float maxSupply = 100;
    public float lossRate;
    public bool active = true;

    private float supply;

    void Start()
    {
        supply = maxSupply;
    }

    void FixedUpdate()
    {
        if (!active) return;

        supply -= lossRate * Time.deltaTime;
        slider.value = supply / maxSupply;

        if (supply <= 0)
        {
            if (playerController.collectible) playerController.collectible = null;
            Exit();
        }
    }

    public void UpgradeSupply()
    {
        // TODO: get from power up?
        var upgrade = 25;

        var rectTransform = slider.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width + upgrade, rectTransform.rect.height);
        maxSupply += upgrade;
        supply = maxSupply;
    }

    public void Exit()
    {
        if (playerController.collectible) UpgradeSupply();
        playerController.Reset();
        active = false;
    }

    public void Enter()
    {
        SetCameraY(0);
        active = true;
        supply = maxSupply;
    }

    public void SetCameraY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}