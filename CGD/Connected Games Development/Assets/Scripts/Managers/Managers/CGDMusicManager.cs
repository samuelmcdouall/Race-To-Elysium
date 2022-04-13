using UnityEngine;
using UnityEngine.UI;

public class CGDMusicManager : MonoBehaviour
{
    public AudioClip Music;
    public Slider MusicSlider;
    public Slider SoundSlider;
    AudioSource _as;
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.loop = true;
        _as.clip = Music;
        _as.volume = CGDGameSettings.MusicVolume;
        _as.Play();
        MusicSlider.value = CGDGameSettings.MusicVolume;
        SoundSlider.value = CGDGameSettings.SoundVolume;
    }

    public void UpdateMusicVolume(float volume) 
    {
        _as.volume = volume;
    }
}
