using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ToggleInteractor : XRRayInteractor
{
    public InputActionProperty gripAction;  // Assign this in Inspector to your Select (Grip) action

    private bool wasSelectPressedLastFrame = false;
    private IXRSelectInteractable currentToggleSelection = null;

    private readonly List<IXRInteractable> targets = new();

    protected override void Awake()
    {
        base.Awake();
        gripAction.action.Enable();
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (gripAction.action == null)
            return;

        bool isSelectPressed = gripAction.action.ReadValue<float>() > 0.5f;

        if (isSelectPressed && !wasSelectPressedLastFrame)
        {
            if (currentToggleSelection == null)
            {
                // Get valid targets
                targets.Clear();
                interactionManager.GetValidTargets(this, targets);

                if (targets.Count > 0 && targets[0] is IXRSelectInteractable interactable)
                {
                    interactionManager.SelectEnter(this, interactable);
                    currentToggleSelection = interactable;
                }
            }
            else
            {
                // Deselect current
                interactionManager.SelectExit(this, currentToggleSelection);
                currentToggleSelection = null;
            }
        }

        wasSelectPressedLastFrame = isSelectPressed;
    }
}
