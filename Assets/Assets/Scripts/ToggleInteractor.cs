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

    public Transform overrideAttachTransform;

    public Transform XROrigin;
    public bool useOverrideAttach = false;
    public LayerMask teleportLayer;
    
    private bool isAwaitingDirection = false;
    private Vector3 teleportPointA;

    public GameObject teleportMarker;
    private GameObject currentTeleportMarker;

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
            Ray ray = new Ray(rayOriginTransform.position, rayOriginTransform.forward);
            RaycastHit hit;
            
            if(isAwaitingDirection){
                if (Physics.Raycast(ray, out hit, maxRaycastDistance))
                {
                    Vector3 teleportDirection = hit.point - teleportPointA;
                    teleportDirection.y = 0f;

                    if (teleportDirection.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(teleportDirection.normalized);
                        TeleportTo(teleportPointA, targetRotation);
                    }
                }

                isAwaitingDirection = false;
            }
            else
            {
                targets.Clear();
                interactionManager.GetValidTargets(this, targets);

                if (targets.Count > 0 && targets[0] is IXRSelectInteractable interactable)
                {
                    if (currentToggleSelection == null)
                    {
                        interactionManager.SelectEnter(this, interactable);
                        currentToggleSelection = interactable;
                    }else{
                        interactionManager.SelectExit(this, currentToggleSelection);
                        currentToggleSelection = null;
                        useOverrideAttach = false;
                    }
                }
                else if (Physics.Raycast(ray, out hit, maxRaycastDistance, teleportLayer))
                {
                    teleportPointA = hit.point;
                    isAwaitingDirection = true;
                    // TeleportTo(hit.point);

                    Vector3 marker = teleportPointA;
                    marker.y = 0.1f;
                    currentTeleportMarker = Instantiate(teleportMarker, marker, Quaternion.identity);
                }
            }
            
        }

        wasSelectPressedLastFrame = isSelectPressed;
    }

    public override Transform GetAttachTransform(IXRInteractable interactable)
    {
        if (useOverrideAttach && overrideAttachTransform != null)
        {
            return overrideAttachTransform;
        }

        return base.GetAttachTransform(interactable);
    }

    private void TeleportTo(Vector3 destination, Quaternion rotation)
    {
        Transform current = XROrigin.transform;
        // Camera camera = XROrigin.GetComponent<Camera>();

        // Vector3 cameraOffset = current.position - Camera.main.transform.position;
        // cameraOffset.y = 0f;  // Keep user grounded

        // current.position = destination;
        current.SetPositionAndRotation(destination, rotation);

         if (currentTeleportMarker != null)
         {
             Destroy(currentTeleportMarker);
         }
    }
}
