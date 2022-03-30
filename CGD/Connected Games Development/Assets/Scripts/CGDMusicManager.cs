using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDMusicManager : MonoBehaviour
{
    // Start is called before the first frame update, todo maybe change to just music manager so can use in other scenes
    AudioSource _as;
    public AudioClip MainMusic;
    public Slider MusicSlider;
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.loop = true;
        _as.clip = MainMusic;
        _as.volume = CGDGameSettings.MusicVolume;
        _as.Play();
        MusicSlider.value = CGDGameSettings.MusicVolume;
    }

    public void UpdateMusicVolume(float volume) 
    {
        _as.volume = volume;
    }
}
