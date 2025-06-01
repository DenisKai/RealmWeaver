using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketAlignment : XRSocketInteractor
{
    public override Transform GetAttachTransform(IXRInteractable interactable)
    {
        var temp_trans = transform;
        temp_trans.rotation = interactable.transform.rotation;
        return temp_trans;
    }
}
