using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SliderXAxis : SliderBase
{
    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(slider.value);
    }

    void OnSliderValueChanged(float value)
    {
        foreach(Transform t in interactors) {
            if (t != null)
            {
                t.localScale = new Vector3(value * 0.025f, t.localScale.y, t.localScale.z);
            }
        }
    }
}
