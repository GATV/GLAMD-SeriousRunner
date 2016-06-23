using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeScript : MonoBehaviour
{
    public AudioSource audioCon;
    public AudioSource audioFXCon;
    public Slider musicUISlider;
    public Slider soundfxUISlider;
    public Toggle muteUIToggle;

    void Start()
    {
        musicUISlider.value = audioCon.volume;
        soundfxUISlider.value = audioFXCon.volume;
    }

    public void VolumeUpdate()
    {
        if (!muteUIToggle.isOn)
        {
            audioCon.volume = musicUISlider.value;
            audioFXCon.volume = soundfxUISlider.value;
        }
    }

    public void MuteAllVolume()
    {
        if (muteUIToggle.isOn)
        { AudioListener.volume = 0; }
        else
        {
            AudioListener.volume = 1;

        }
    }
}