using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public Renderer backgroundRenderer;
    public Image toBlack;
    public GameObject menu;
    public Slider slider;
    public ObstacleSpawner obstacleSpawner;

    public bool active;
    public float maxSupply = 100;
    public float lossRate;
    public float obstacleSpawnTime = 10;
    public float obstacleSpawnChance = 0.75f;
    public float fadeSpeed = 20;

    private float supply;
    private bool ready = true;
    private SoundController soundController;
    private float obstacleSpawnCooldown;
    private List<GameObject> obstacles = new List<GameObject>();

    void Awake()
    {
        soundController = GetComponent<SoundController>();
        supply = maxSupply;
        obstacleSpawnCooldown = obstacleSpawnTime;
    }

    void Update()
    {
        SpawnObstacle();

        if (!active && ready && Input.anyKeyDown)
        {
            Enter();
        }
    }

    private void SpawnObstacle()
    {
        if (!active) return;

        obstacleSpawnCooldown -= Time.deltaTime;
        if (obstacleSpawnCooldown > 0) return;

        if (Random.value <= obstacleSpawnChance)
        {
            var bounds = backgroundRenderer.bounds;

            var xMin = bounds.min.x;
            var xMax = bounds.max.x;
            var yMin = bounds.min.y + bounds.size.y * 1.2f;
            var yMax = bounds.max.y + bounds.size.y * 1.2f;

            var randomPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
            var newObstacle = obstacleSpawner.Spawn(randomPosition);
            obstacles.Add(newObstacle);
        }

        obstacleSpawnCooldown = obstacleSpawnTime;
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
        StartCoroutine(FillSupplyBar(true));
    }

    public void Exit()
    {
        active = false;
        StartCoroutine("FadeToBlack");
    }

    public void Enter()
    {
        StartCoroutine(playerController.SetRetractionDelay());
        menu.SetActive(false);
        active = true;
        supply = maxSupply;
    }

    public void SetCameraY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    private IEnumerator FillSupplyBar(bool success)
    {
        var fill = slider.GetComponentsInChildren<Image>()[1];

        fill.color = success ? Color.green : Color.red;

        while (slider.value < 1)
        {
            slider.value += Time.deltaTime * 2;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        fill.color = Color.white;
    }
    private IEnumerator FadeToBlack()
    {
        ready = false;

        while (toBlack.color.a < 1)
        {
            toBlack.color = new Color(0,0,0,toBlack.color.a + Time.deltaTime * fadeSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Setup();

        yield return new WaitForSeconds(0.5f);

        while (toBlack.color.a > 0)
        {
            toBlack.color = new Color(0,0,0,toBlack.color.a - Time.deltaTime * fadeSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        menu.SetActive(true);
        ready = true;
    }

    private void Setup()
    {
        SetCameraY(0);

        if (playerController.collectible)
        {
            if (playerController.collectible.name == "PowerCell") EndGame();
            UpgradeSupply(playerController.collectible);
        }
        else StartCoroutine(FillSupplyBar(false));

        playerController.Reset();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        obstacles = new List<GameObject>();
    }

    private void EndGame()
    {
        SceneManager.LoadScene("End");
    }
}