using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CellBehaviour : MonoBehaviour
{
    public Color defaultColor;
    public Color hoverColor;
    public Color selectedColor;

    [SerializeField] private AudioClip hoverSoundClip;

    private MeshRenderer meshRenderer;
    private XRGrabInteractable grabInteractable;


void Start() {
    if (hoverSoundClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(hoverSoundClip, transform, 1f);
            }
}

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = defaultColor;

        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.firstSelectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.firstHoverEntered.AddListener(OnHoverEnter);
        grabInteractable.lastHoverExited.AddListener(OnHoverExit);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        meshRenderer.material.color = selectedColor;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (!grabInteractable.isHovered)
        {
            meshRenderer.material.color = defaultColor;
        }
        else
        {
            meshRenderer.material.color = hoverColor;
        }
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (!grabInteractable.isSelected)
        {
            meshRenderer.material.color = hoverColor;
            if (hoverSoundClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(hoverSoundClip, transform, 1f);
            }
        }
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        if (!grabInteractable.isSelected)
        {
            meshRenderer.material.color = defaultColor;
        }
        else
        {
            meshRenderer.material.color = selectedColor;
        }
    }
}
