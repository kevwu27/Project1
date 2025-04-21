using UnityEngine;
using UnityEngine.InputSystem;

public class RightControllerMovement : MonoBehaviour {
    public float moveSpeed = 1.5f;

    public InputActionProperty inputActionProperty;

    private Transform xrOrigin;
    
    void Start() {
        xrOrigin = transform.parent.parent.parent;
    }

    void Update() {
        Vector2 stick = inputActionProperty.action.ReadValue<Vector2>();

        if (stick.magnitude > 0.1f) // Only move if joystick is moved past a certain threshold
        {
            Transform rightController = xrOrigin.transform; // Reference to your right controller's transform
            Vector3 forward = rightController.forward; // Direction of movement along the forward axis (relative to controller's orientation)
            forward.y = 0; // Ensure we don't move vertically (keep the y-axis zeroed)
            forward.Normalize(); // Normalize to get only direction, without magnitude

            Vector3 right = rightController.right; // Direction of movement along the right axis (relative to controller's orientation)
            right.y = 0; // Keep the movement on the ground (no vertical movement)
            right.Normalize(); // Normalize for the same reason

            // Calculate movement based on joystick inputs
            Vector3 move = forward * stick.y + right * stick.x;

            // Move the camera or player (xrOrigin)
            xrOrigin.position += moveSpeed * Time.deltaTime * move;
        }

        
    }
}