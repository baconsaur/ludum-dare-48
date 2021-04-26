using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public float showImageForSeconds = 5;
    public float fadeSpeed = 2;
    public Image image;
    public Sprite[] sprites;
    void Start()
    {
        StartCoroutine("PlayScene");
    }

    IEnumerator PlayScene()
    {
        foreach (var sprite in sprites)
        {
            image.sprite = sprite;
            while (image.color.a < 1)
            {
                image.color = new Color(1,1,1,image.color.a + Time.deltaTime * fadeSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(showImageForSeconds);

            while (image.color.a > 0)
            {
                image.color = new Color(1,1,1,image.color.a - Time.deltaTime * fadeSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene("Main");
    }
}
