using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource _audioSrc;
    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private Sprite[] muteIcons;

    private bool _sliderActive = false;

    private void Start()
    {
        _audioSrc = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        _audioSrc.clip = audioClips[clipIndex];
        _audioSrc.Play();
    }

    public void ToggleSlider()
    {
        _sliderActive = !_sliderActive;
        volumeSlider.SetActive(_sliderActive);
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        muteButton.GetComponent<Image>().sprite = volume == -80f
            ? muteIcons[(int) MuteIconState.Muted]
            : muteIcons[(int) MuteIconState.Unmuted];
    }
}