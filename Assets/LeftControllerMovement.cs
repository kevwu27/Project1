using UnityEngine;
using UnityEngine.InputSystem;

public class LeftControllerMovement : MonoBehaviour {
    public float rotationSpeed = 1.5f;

    public InputActionProperty inputActionProperty;

    private Transform xrOrigin;
    
    void Start() {
        xrOrigin = transform.parent.parent.parent;
    }

    void Update() {
        Vector2 stick = inputActionProperty.action.ReadValue<Vector2>();

        float yaw = stick.x * rotationSpeed * Time.deltaTime;
        xrOrigin.transform.Rotate(0, yaw, 0);
        
    }
}