using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    private AudioSource audioSource;
    private float defaultVolume = 0.2f; // 20% volume

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();

        // Set the volume to 30% at the start
        audioSource.volume = defaultVolume;

        // Also update the slider's position to reflect this
        volumeSlider.value = defaultVolume;

        // Add a listener to call a method every time the value changes
        volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    void OnSliderValueChanged()
    {
        audioSource.volume = volumeSlider.value;
    }
}
