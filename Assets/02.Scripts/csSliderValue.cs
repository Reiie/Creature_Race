using UnityEngine;
using System.Collections;

public class csSliderValue : MonoBehaviour {

    private UISlider slider;

    void Awake()
    {
        slider = GetComponent<UISlider>();
    }

    public void OnSliderChanged()
    {
       // Debug.Log(slider.value);
    }
}
