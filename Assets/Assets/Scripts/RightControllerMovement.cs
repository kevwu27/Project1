using UnityEngine;
using UnityEngine.InputSystem;

public class RightControllerMovement : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public InputActionProperty inputActionProperty;

    private Transform xrOrigin;
    private Transform headTransform;

    void Start()
    {
        xrOrigin = transform.parent.parent.parent; // XR Origin
        headTransform = Camera.main.transform;     // Main camera represents the headset
    }

    void Update()
    {
        Vector2 stick = inputActionProperty.action.ReadValue<Vector2>();

        if (stick.magnitude > 0.1f)
        {
            // Move relative to headset forward direction
            Vector3 forward = headTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = headTransform.right;
            right.y = 0f;
            right.Normalize();

            Vector3 move = forward * stick.y + right * stick.x;
            xrOrigin.position += move * moveSpeed * Time.deltaTime;
        }
    }
}
