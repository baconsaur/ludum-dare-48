using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string menu = "Menu";
    public string intro = "Intro";
    public string game = "Main";
    public string end = "End";

    public void LoadMenu()
    {
        SceneManager.LoadScene(menu);
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene(intro);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(game);
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene(end);
    }
}