using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TwoHandManipulation : MonoBehaviour
{
    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;
    public Transform leftController;
    public Transform rightController;

    private bool leftTriggerPressed;
    private bool rightTriggerPressed;

    private float initialDistance;
    private Vector3 initialScale;

    private XRGrabInteractable interactable;

    // Flag to know if the object is selected
    private bool isSelected = false;

    public enum ControllerSide { None, Left, Right }
    private ControllerSide selectedBy = ControllerSide.None;

    private Quaternion initialRotationOffset;
    
    private Quaternion lastOtherControllerRotation;

    void OnEnable()
    {
        leftTrigger.action.Enable();
        rightTrigger.action.Enable();

        interactable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // Skip manipulation if the object is not selected
        if (!isSelected)
            return;

        leftTriggerPressed = leftTrigger.action.ReadValue<float>() > 0.5f;
        rightTriggerPressed = rightTrigger.action.ReadValue<float>() > 0.5f;

        // if (leftTriggerPressed || rightTriggerPressed)
        // {
        //     interactable.trackPosition = false;
        //     interactable.trackRotation = true;

        // } 

        // if (leftTriggerPressed && rightTriggerPressed)
        // {
        //     interactable.trackPosition = false;
        //     interactable.trackRotation = false;

        //     ScaleFromControllerDistance();
        // }
        if (leftTriggerPressed && rightTriggerPressed)
        {
            // Two-hand scale
            interactable.trackPosition = false;
            interactable.trackRotation = false;

            ScaleFromControllerDistance();
        }
        else if ((selectedBy == ControllerSide.Left && rightTriggerPressed) ||
                 (selectedBy == ControllerSide.Right && leftTriggerPressed))
        {
            // One controller selected the object, the other trigger is pressed: rotate
            Transform otherController = (selectedBy == ControllerSide.Left) ? rightController : leftController;
            Quaternion currentRotation = otherController.rotation;
            Quaternion deltaRotation = currentRotation * Quaternion.Inverse(lastOtherControllerRotation);

            transform.rotation = deltaRotation * transform.rotation;

            lastOtherControllerRotation = currentRotation;
            Debug.Log(otherController.rotation);
            interactable.trackPosition = true;
            interactable.trackRotation = false;
            // RotateWithOffHand();
        }
        else
        {
            // Reset mode
            interactable.trackPosition = true;
            interactable.trackRotation = false;
            initialDistance = 0f;
        }

    }

    // private void RotateWithOffHand()
    // {
    //     Transform offHand = selectedBy == ControllerSide.Left ? rightController : leftController;

    //     if (initialRotationOffset == Quaternion.identity)
    //     {
    //         // Store initial rotation difference
    //         objectInitialRotation = transform.rotation;
    //         initialRotationOffset = Quaternion.Inverse(offHand.rotation) * transform.rotation;
    //         return;
    //     }
    //     Debug.Log(offHand.rotation);
    //     transform.rotation = offHand.rotation * initialRotationOffset;
    // }

    private void ScaleFromControllerDistance()
    {
        float currentDistance = Vector3.Distance(leftController.position, rightController.position);
        Debug.Log($"Current Distance: {currentDistance}");

        if (initialDistance == 0f)
        {
            initialDistance = currentDistance;
            initialScale = transform.localScale;
            Debug.Log($"Initialized: Distance = {initialDistance}, Scale = {initialScale}");
            return;
        }

        float scaleRatio = currentDistance / initialDistance;
        Vector3 newScale = initialScale * scaleRatio;
        Debug.Log($"Scale Ratio: {scaleRatio}, New Scale: {newScale}");

        transform.localScale = newScale;
    }


    void LateUpdate()
    {
        // Reset the values when the manipulation ends
        if (!leftTriggerPressed && !rightTriggerPressed)
        {
            interactable.trackPosition = true;
            interactable.trackRotation = false;
            initialDistance = 0f;
        }
    }


    // Call this when the object is selected
    public void StartManipulating(ControllerSide side)
    {
        isSelected = true;
        selectedBy = side;

        Debug.Log(side);
        if (side == ControllerSide.Left)
            lastOtherControllerRotation = rightController.rotation;
        else if (side == ControllerSide.Right)
            lastOtherControllerRotation = leftController.rotation;
        }

    // Call this when the object is deselected
    public void StopManipulating()
    {
        isSelected = false;
        selectedBy = ControllerSide.None;
        initialRotationOffset = Quaternion.identity;
    }
}
