using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private string type;


    public void MixerControl(float sliderValue)
    {
        audioMixer.SetFloat(type, sliderValue);
    }
}
