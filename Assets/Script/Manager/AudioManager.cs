using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("------------Audio Source-----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------Audio Clip-----------")]
    public AudioClip background;
    public AudioClip endGame;
    public AudioClip levelUp;
    public AudioClip pickUp;
    public AudioClip attack;
    public AudioClip takeDame;
    public AudioClip click;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void Click()
    {
        PlaySFX(click);
    }
}
