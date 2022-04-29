using UnityEngine;
using UnityEngine.UI;

public class CGDMusicManager : MonoBehaviour
{
    public AudioClip IntroMusic;
    public AudioClip LoopMusic;
    public Slider MusicSlider;
    public Slider SoundSlider;
    AudioSource _as;
    bool _loopMusicPlaying;
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _loopMusicPlaying = false;
        _as.clip = IntroMusic;
        _as.volume = CGDGameSettings.MusicVolume;
        _as.Play();
        MusicSlider.value = CGDGameSettings.MusicVolume;
        SoundSlider.value = CGDGameSettings.SoundVolume;
    }

    void Update()
    {
        if (!_loopMusicPlaying && !_as.isPlaying)
        {
            _as.Stop();
            _as.loop = true;
            _as.clip = LoopMusic;
            _as.volume = CGDGameSettings.MusicVolume;
            _as.Play();
            _loopMusicPlaying = true;
        }    
    }

    public void UpdateMusicVolume(float volume) 
    {
        _as.volume = volume;
    }
}
