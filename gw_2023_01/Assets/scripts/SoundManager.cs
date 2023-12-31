using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _effectsSource;

    private AudioClip _clip;

    public static string[] ObjectSounds = { "tone01", "tone02", "tone03", "tone04" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

    }

    public void PlaySound(AudioClip clip)
    {
        if (!GameManager.IdleState)
        {
            _effectsSource.PlayOneShot(clip);
        }
        
    }


    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    

    public void MuteToggle(bool muted)
    {
        _effectsSource.mute = !_effectsSource.mute;
    }

    public void ButtonClick()
    {
        _clip = (AudioClip)Resources.Load("button_click_01");
        _effectsSource.PlayOneShot(_clip);
    }
}
