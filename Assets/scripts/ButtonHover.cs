using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    private Button button;

    void OnTriggerEnter(Collider other) {
        button.targetGraphic.color = button.colors.highlightedColor;
    }

    void OnTriggerExit(Collider other) {
        button.targetGraphic.color = button.colors.normalColor;
    }
}
