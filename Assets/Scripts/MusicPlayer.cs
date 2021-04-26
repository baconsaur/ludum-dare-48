using UnityEngine;
public class MusicPlayer : MonoBehaviour
{
    static bool musicBegin;
    public AudioSource music;
    void Awake()
    {
        if (!musicBegin) {
            music.Play ();
            DontDestroyOnLoad (gameObject);
            musicBegin = true;
        }
    }
}