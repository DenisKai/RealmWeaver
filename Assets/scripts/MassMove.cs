using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class MassMove : MonoBehaviour
{
    private XRDirectInteractor m_interactor;
    private HashSet<IXRHoverInteractable> m_hovered;
    private HashSet<IXRSelectInteractable> m_selected;
    private HashSet<IXRSelectInteractable> m_selectedConfirmed;

    void Awake()
    {
        m_interactor = GetComponent<XRDirectInteractor>();
        m_hovered = new HashSet<IXRHoverInteractable>();
        m_selected= new HashSet<IXRSelectInteractable>();
        m_selectedConfirmed = new HashSet<IXRSelectInteractable>();

        //Listeners
        m_interactor.hoverEntered.AddListener(HoverStart);
        m_interactor.hoverExited.AddListener(HoverEnd);
        m_interactor.selectEntered.AddListener(SelectStart);
        m_interactor.selectExited.AddListener(SelectEnd);
    }

    private void HoverStart(HoverEnterEventArgs args) {
        m_hovered.Add(args.interactableObject);        
    }

    private void HoverEnd(HoverExitEventArgs args) {
        m_hovered.Remove(args.interactableObject);
    }

    private void SelectStart(SelectEnterEventArgs args) {
        XRDirectInteractor interactor = (XRDirectInteractor) args.interactorObject;
        m_selected = new HashSet<IXRSelectInteractable>(
            m_hovered.OfType<IXRSelectInteractable>()
        );
        m_selectedConfirmed.Clear();

        foreach (var interactable in m_selected) {
            if (interactable != args.interactableObject) {
                var selectedInteractable = (IXRSelectInteractable) interactable;
                interactor.interactionManager.SelectEnter(interactor, selectedInteractable);

                if (interactor.IsSelecting(selectedInteractable)) {
                    m_selectedConfirmed.Add(selectedInteractable);
                }
            }
        }
    }

    private void SelectEnd(SelectExitEventArgs args) {
        XRDirectInteractor interactor = (XRDirectInteractor) args.interactorObject;

        foreach (var interactable in m_selectedConfirmed) {
            var selectedInteractable = (IXRSelectInteractable) interactable;

            if (interactable != args.interactableObject && interactor.IsSelecting(selectedInteractable)) {
                interactor.interactionManager.SelectExit(interactor, selectedInteractable);
            }
        }

        m_selected.Clear();
        m_selectedConfirmed.Clear();
    }
}
