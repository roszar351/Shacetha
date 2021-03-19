using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        float temp;
        AudioManager.instance.mixer.GetFloat("masterVolume", out temp);
        masterSlider.value = temp;
        AudioManager.instance.mixer.GetFloat("musicVolume", out temp);
        musicSlider.value = temp;
        AudioManager.instance.mixer.GetFloat("effectsVolume", out temp);
        effectsSlider.value = temp;
    }
}
