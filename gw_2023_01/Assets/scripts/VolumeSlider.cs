using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(val  => SoundManager.Instance.ChangeMasterVolume(val));
    }

}
