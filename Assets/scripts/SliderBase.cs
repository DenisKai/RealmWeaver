using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class SliderBase : MonoBehaviour
{
    public Slider slider;
    public List<Transform> interactors = new List<Transform>();
}
