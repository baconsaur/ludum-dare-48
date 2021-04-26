using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource grabItem;
    public AudioSource loseItem;
    public AudioSource increaseOxygen;
    public AudioSource boost;

    public void PlayGrabItem()
    {
        grabItem.Play();
    }
    public void PlayLoseItem()
    {
        loseItem.Play();
    }
    public void PlayIncreaseOxygen()
    {
        increaseOxygen.Play();
    }
    public void StartBoost()
    {
        if (boost.isPlaying) return;
        boost.Play();
    }
    public void StopBoost()
    {
        if (!boost.isPlaying) return;
        boost.Stop();
    }
}
