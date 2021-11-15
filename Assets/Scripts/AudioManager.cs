using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioClip[] audioClips;
    private AudioSource audioSrc;
    public Sprite[] muteIcons;
    public GameObject muteButton;
    public GameObject slider;

    private bool sliderActive = false;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        audioSrc.clip = audioClips[clipIndex];
        audioSrc.Play();
    }

    public void ToggleSlider()
    {
        sliderActive = !sliderActive;
        slider.SetActive(sliderActive);
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        if (volume == -80f)
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconState.muted];
        }
        else
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconState.unmuted];
        }
    }
}