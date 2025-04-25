using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CustomLeftToggleDirectInteractor : XRDirectInteractor
{
    public InputActionProperty selectAction;  // Assign to Trigger or Grip in Inspector

    private bool wasPressedLastFrame = false;
    private IXRSelectInteractable currentToggleSelection = null;

    private readonly List<IXRInteractable> targets = new();

    public Transform overrideAttachTransform;
    public bool useOverrideAttach = false;

    protected override void Awake()
    {
        base.Awake();
        selectAction.action.Enable();
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (selectAction.action == null)
            return;

        bool isPressed = selectAction.action.ReadValue<float>() > 0.5f;

        if (isPressed && !wasPressedLastFrame)
        {
            // On press
            if (currentToggleSelection == null)
            {
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
                interactionManager.SelectExit(this, currentToggleSelection);
                currentToggleSelection = null;
                useOverrideAttach = false;
            }
        }

        wasPressedLastFrame = isPressed;
    }

    public override Transform GetAttachTransform(IXRInteractable interactable)
    {
        if (useOverrideAttach && overrideAttachTransform != null)
        {
            return overrideAttachTransform;
        }

        return base.GetAttachTransform(interactable);
    }
}