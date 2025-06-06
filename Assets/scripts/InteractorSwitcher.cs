using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class InteractorSwitcher : MonoBehaviour
{
    [Header("Interactors - Left")]
    public GameObject L_Point;
    public GameObject L_Box;
    public GameObject L_Circle;

    [Header("Interactors - Right")]
    public GameObject R_Point;
    public GameObject R_Box;
    public GameObject R_Circle;

    [Header("Toggles")]
    public Toggle point;
    public Toggle rect;
    public Toggle circle;

    [Header("Sliders")]
    public GameObject xAxis;
    public GameObject zAxis;

    [Header("Slopebuttons")]
    public Toggle slopeOption;

    [Header("MassMove Script")]
    public MassMove L_massMove_rect;
    public MassMove L_massMove_circle;
    public MassMove R_massMove_rect;
    public MassMove R_massMove_cirlce;

    public enum Interactormode {Point, Rectangle, Circle};

    // Start is called before the first frame update
    void Start()
    {
        ToggleInteractor(point, Interactormode.Point);
        point.onValueChanged.AddListener(delegate {ToggleInteractor(point, Interactormode.Point);});
        rect.onValueChanged.AddListener(delegate {ToggleInteractor(rect, Interactormode.Rectangle);});
        circle.onValueChanged.AddListener(delegate {ToggleInteractor(circle, Interactormode.Circle);});
        slopeOption.onValueChanged.AddListener(ToggleSlopeMode);
    }

    private void ToggleInteractor(Toggle toggle, Interactormode mode) {
        if (toggle.isOn) {
            L_Point.SetActive(false);
            L_Box.SetActive(false);
            L_Circle.SetActive(false);
            R_Point.SetActive(false);
            R_Box.SetActive(false);
            R_Circle.SetActive(false);

            if (mode == Interactormode.Point) {
                L_Point.SetActive(true);
                R_Point.SetActive(true);
                xAxis.SetActive(false);
                zAxis.SetActive(false);
                slopeOption.gameObject.SetActive(false);
            } else if (mode == Interactormode.Rectangle) {
                L_Box.SetActive(true);
                R_Box.SetActive(true);
                xAxis.SetActive(true);
                zAxis.SetActive(true);
                slopeOption.gameObject.SetActive(true);
            } else if (mode == Interactormode.Circle) {
                L_Circle.SetActive(true);
                R_Circle.SetActive(true);
                xAxis.SetActive(true);
                zAxis.SetActive(true);
                slopeOption.gameObject.SetActive(true);
            } else {
                Debug.Log("Invalid collider selected.");
            }
        }
    }

    public void ToggleSlopeMode(bool value)
    {
        L_massMove_rect.isSlopeMode = value;
        L_massMove_circle.isSlopeMode = value;
        R_massMove_rect.isSlopeMode = value;
        R_massMove_cirlce.isSlopeMode = value;
    }
}
