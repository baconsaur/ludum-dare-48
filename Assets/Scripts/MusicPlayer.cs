using UnityEngine;
public class MusicPlayer : MonoBehaviour
{
    private static bool musicOff;
    static bool musicBegin;
    public AudioSource music;
    void Awake()
    {
        if (!musicBegin && !musicOff) {
            music.Play ();
            DontDestroyOnLoad (gameObject);
            musicBegin = true;
        }
    }

    public void ToggleMusic()
    {
        if (musicOff)
        {
            musicOff = false;
            music.Play();
        }
        else
        {
            musicOff = true;
            music.Stop();
        }
    }
}