using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_AudioSliderHandler : MonoBehaviour {
    public bool isMusicSlider;
    Slider slider;

	void Start () {
        slider = GetComponent<Slider>();
        if (isMusicSlider)
        {
            slider.value = Manager.MusicVolume;
        }
        else
        {
            slider.value = Manager.AudioVolume;
        }

        slider.onValueChanged.AddListener(OnValueChanged);
    }
	
    void OnValueChanged(float val)
    {
        if (isMusicSlider)
        {
            Manager.MusicVolume = val;
        }
        else
        {
            Manager.AudioVolume = val;
        }
    }
}
