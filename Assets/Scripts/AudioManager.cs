using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    public void PlaySound(int clipIndex)
    {
        gameObject.GetComponent<AudioSource>().clip = audioClips[clipIndex];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
