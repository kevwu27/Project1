using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CustomLeftToggleDirectInteractor : XRDirectInteractor
{
    public InputActionProperty selectAction;  // Assign to Trigger or Grip in Inspector

    private bool wasPressedLastFrame = false;
    private IXRSelectInteractable currentToggleSelection = null;

    private readonly List<IXRInteractable> validTargets = new();

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
                validTargets.Clear();
                interactionManager.GetValidTargets(this, validTargets);

                foreach (var target in validTargets)
                {
                    if (target is IXRSelectInteractable interactable)
                    {
                        interactionManager.SelectEnter(this, interactable);
                        currentToggleSelection = interactable;
                        break;
                    }
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
