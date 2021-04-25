using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource grabItem;
    public AudioSource loseItem;
    public AudioSource increaseOxygen;

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
}
