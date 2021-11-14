using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MuteIconStates
{
    muted = 0,
    unmuted = 1
}

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSrc;
    private bool audioIsMuted = false;
    public Sprite[] muteIcons;
    public GameObject muteButton;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        if (!audioIsMuted)
        {
            audioSrc.clip = audioClips[clipIndex];
            audioSrc.Play();
        }
    }
    public void ToggleMute()
    {
        audioIsMuted = !audioIsMuted;
        if (audioIsMuted)
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconStates.muted];
        }
        else
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconStates.unmuted];
        }
        
    }
}
