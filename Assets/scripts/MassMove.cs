using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class MassMove : MonoBehaviour
{
    public bool isSlopeMode = false;
    private XRDirectInteractor m_interactor;
    private HashSet<IXRHoverInteractable> m_hovered;
    private HashSet<IXRSelectInteractable> m_selected;
    private HashSet<IXRSelectInteractable> m_selectedConfirmed;

    void Awake()
    {
        m_interactor = GetComponent<XRDirectInteractor>();
        m_hovered = new HashSet<IXRHoverInteractable>();
        m_selected = new HashSet<IXRSelectInteractable>();
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

        if (interactor.interactablesSelected.Count > 1)
            return;
        
        m_selected = new HashSet<IXRSelectInteractable>(
            m_hovered.OfType<IXRSelectInteractable>()
        );
        m_selectedConfirmed.Clear();

        foreach (var interactable in m_selected) {
            var selectedInteractable = (IXRSelectInteractable) interactable;
            interactor.interactionManager.SelectEnter(interactor, selectedInteractable);

            if (interactor.IsSelecting(selectedInteractable)) {
                m_selectedConfirmed.Add(selectedInteractable);
            }
        }
    }

    private void SelectEnd(SelectExitEventArgs args) {
        XRDirectInteractor interactor = (XRDirectInteractor)args.interactorObject;

        if (interactor.interactablesSelected.Count > 1)
            return;

        var m_slopeSelection = new HashSet<IXRSelectInteractable>(m_selectedConfirmed);
        foreach (var interactable in m_selectedConfirmed)
        {
            var selectedInteractable = (IXRSelectInteractable)interactable;
            if (interactor.IsSelecting(selectedInteractable))
            {
                interactor.interactionManager.SelectExit(interactor, selectedInteractable);
            }
        }

        m_selected.Clear();
        m_selectedConfirmed.Clear();

        if (isSlopeMode)
        {
            StartCoroutine(WaitUntilAllDeselected(m_slopeSelection, interactor));   
        }
    }

    private IEnumerator WaitUntilAllDeselected(HashSet<IXRSelectInteractable> m_slopeSelection, XRDirectInteractor interactor)
    {
        var interactables = m_slopeSelection
            .Select(x => (x as XRBaseInteractable))
            .Where(x => x != null)
            .ToList();

        // wait until all cells are deselected
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.025f);
        yield return new WaitUntil(() => interactor.interactablesSelected.Count == 0);
        yield return new WaitUntil(() => interactables.All(i => !i.isSelected));
        ApplySlopeToAllSelectedCells(m_slopeSelection);
    }

    private void ApplySlopeToAllSelectedCells(HashSet<IXRSelectInteractable> m_slopeSelection)
    {
        if (m_slopeSelection.Count == 0)
            return;

        // Get valid cell transforms
        var cells = m_slopeSelection
            .Where(x => ((MonoBehaviour)x).transform.CompareTag("Cell"))
            .Select(x => ((MonoBehaviour)x).transform)
            .ToList();

        if (cells.Count == 0) return;

        // Find center cell based on average position (instead of midpoint index)
        Vector3 center = Vector3.zero;
        foreach (var t in cells)
            center += t.position;
        center /= cells.Count;

        // Use the cell closest to center as middle cell
        Transform midCell = cells.OrderBy(t => Vector3.Distance(t.position, center)).First();

        float baseY = midCell.parent.position.y;
        float topY = midCell.position.y;
        float slopeRange = topY - baseY;

        // Get the maximum distance from center to normalize the slope factor
        float maxDistance = cells.Max(c => Vector3.Distance(c.position, midCell.position));
        if (maxDistance == 0) maxDistance = 1; // Avoid division by zero

        foreach (Transform cell in cells)
        {
            if (cell == midCell) continue;

            float distance = Vector3.Distance(cell.position, midCell.position);
            float slopeFactor = distance / maxDistance;

            float targetY = topY - slopeRange * slopeFactor;

            Vector3 newPos = cell.position;
            newPos.y = targetY;
            cell.position = newPos;
        }

        m_slopeSelection.Clear();
    }
}
