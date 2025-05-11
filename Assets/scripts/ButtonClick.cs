using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    private Button button;

    void OnTriggerEnter(Collider other) {
        button.targetGraphic.color = button.colors.pressedColor;
        button.onClick.Invoke();
    }

    void OnTriggerExit(Collider other) {
        button.targetGraphic.color = button.colors.highlightedColor;
    }
}
