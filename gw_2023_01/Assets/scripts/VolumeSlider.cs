
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(volumeSlider.value);

        volumeSlider.onValueChanged.AddListener(val  => SoundManager.Instance.ChangeMasterVolume(val));
    }

}
