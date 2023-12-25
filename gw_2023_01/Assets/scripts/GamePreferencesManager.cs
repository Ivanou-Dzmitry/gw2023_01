using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GamePreferencesManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle soundToggle;

    private const string VolumeLevel = "Volume";
    private const string SoundToggle = "Sound_Toggle";

    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
    }

    // Update is called once per frame
    void OnApplicationQuit ()
    {
        SavePrefs();
    }

    public void SavePrefs()
    {
        //Debug.Log(soundToggle.isOn.ToString());

        //volume level
        PlayerPrefs.SetFloat(VolumeLevel, volumeSlider.value);

        //sound on off
        PlayerPrefs.SetString(SoundToggle, soundToggle.isOn.ToString());

        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        //volume level
        var volume = PlayerPrefs.GetFloat(VolumeLevel, 1);
        volumeSlider.value = volume;

        //sound on off
        var sound_toggle = PlayerPrefs.GetString(SoundToggle, "true");
        soundToggle.isOn = Convert.ToBoolean(sound_toggle);
    }

}
