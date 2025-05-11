using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SliderZAxis : SliderBase
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
                t.localScale = new Vector3(t.localScale.x, t.localScale.y, value * 0.025f);
            }
        }
    }
}
