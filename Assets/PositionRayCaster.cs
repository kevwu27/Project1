using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
public class PositionRayCaster : MonoBehaviour
{
    [Header("Ray Settings")]
    public float maxRayLength = 100f;
    public LayerMask interactableLayers;
    public Material highlightMaterial;

    [Header("Input Actions")]
    public InputActionProperty gripAction;

    private LineRenderer lr;
    private XRBaseInteractor directHand;
    private XRGrabInteractable grabbed;
    private XRGrabInteractable hovered;
    private Material originalMat;

    private Rigidbody grabbedRb;
    private Vector3 grabOffsetLocal;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        directHand = GetComponentInParent<XRBaseInteractor>();
    }

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * maxRayLength;

        float gripValue = gripAction.action.ReadValue<float>();
        bool gripDown = gripValue > 0.5f;

        bool hitSomething = Physics.Raycast(start, transform.forward, out RaycastHit hit, maxRayLength, interactableLayers);

        if (hitSomething)
        {
            end = hit.point;
            var target = hit.collider.GetComponentInParent<XRGrabInteractable>();

            if (target == grabbed)
            {
                lr.startColor = Color.green;
                lr.endColor = Color.green;
            }
            else
            {
                lr.startColor = Color.red;
                lr.endColor = Color.red;
                HandleHover(target);
                if (gripDown && grabbed == null && target != null)
                    BeginGrab(target);
            }

            if (!gripDown && grabbed != null)
                EndGrab();
        }
        else
        {
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            HandleHover(null);
            if (!gripDown && grabbed != null)
                EndGrab();
        }

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        if (grabbed != null && grabbedRb != null)
        {
            Vector3 worldOffset = transform.TransformVector(grabOffsetLocal);
            Vector3 targetPos = transform.position + worldOffset;
            grabbedRb.MovePosition(targetPos);
            grabbedRb.MoveRotation(transform.rotation);
        }
    }

    void HandleHover(XRGrabInteractable target)
    {
        if (hovered != null && (hovered != target || grabbed != null))
        {
            ResetHighlight(hovered);
            hovered = null;
        }

        if (target != null && target != grabbed && target != hovered)
        {
            var renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMat = renderer.material;
                renderer.material = highlightMaterial;
                hovered = target;
            }
        }
    }

    void ResetHighlight(XRGrabInteractable target)
    {
        if (target == null || originalMat == null) return;
        var renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = originalMat;
        }
    }

    void BeginGrab(XRGrabInteractable target)
    {
        grabbed = target;

        if (directHand != null && directHand.interactionManager != null)
        {
            directHand.interactionManager.SelectEnter(
                (IXRSelectInteractor)directHand,
                (IXRSelectInteractable)grabbed);
        }

        ResetHighlight(hovered);
        hovered = null;

        grabbedRb = grabbed.GetComponent<Rigidbody>();
        if (grabbedRb != null)
        {
            grabbedRb.isKinematic = true;
        }

        // ✅ Correct offset: keeps object in place when grabbed
        grabOffsetLocal = transform.InverseTransformVector(grabbed.transform.position - transform.position);

        // Optional: suppress Toolkit throw logic
        grabbed.throwOnDetach = false;
        grabbed.trackPosition = true;
        grabbed.trackRotation = true;
        grabbed.movementType = XRBaseInteractable.MovementType.Instantaneous;
    }

    void EndGrab()
    {
        if (grabbed == null) return;

        if (directHand != null && directHand.interactionManager != null)
        {
            directHand.interactionManager.SelectExit(
                (IXRSelectInteractor)directHand,
                (IXRSelectInteractable)grabbed);
        }

        if (grabbedRb != null)
        {
            grabbedRb.velocity = Vector3.zero;
            grabbedRb.angularVelocity = Vector3.zero;
            grabbedRb.isKinematic = false;
        }

        grabbedRb = null;
        grabbed = null;
    }
}
