using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private AudioSource audioSrc;
    public GameObject muteButton;
    public GameObject volumeSlider;
    public AudioClip[] audioClips;
    public Sprite[] muteIcons;

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
        volumeSlider.SetActive(sliderActive);
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        if (volume == -80f)
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconState.Muted];
        }
        else
        {
            muteButton.GetComponent<Image>().sprite = muteIcons[(int)MuteIconState.Unmuted];
        }
    }
}