using UnityEngine;
using UnityEngine.InputSystem;

public class LeftControllerMovement : MonoBehaviour {
    public float rotationSpeed = 1.5f;

    public InputActionProperty inputActionProperty;

    private Transform xrOrigin;
    
    void Start() {
        xrOrigin = transform.parent.parent.parent.transform;
    }

    void Update() {
        Vector2 stick = inputActionProperty.action.ReadValue<Vector2>();

        float yaw = stick.x * rotationSpeed * Time.deltaTime;
        // float pitch = -stick.y * rotationSpeed * Time.deltaTime;

        xrOrigin.Rotate(Vector3.up, yaw, Space.World);
        // xrOrigin.Rotate(xrOrigin.right, pitch, Space.World);
    }
}