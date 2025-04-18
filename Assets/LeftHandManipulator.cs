using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class LeftHandManipulator : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // Reference to the ray on this hand
    [SerializeField] private XRController controller;    // Input from this controller

    private Transform selectedObject = null;
    private enum Mode { None, Rotate, Scale }
    private Mode currentMode = Mode.None;

    void Update()
    {
        // Read trigger & grip input
        controller.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool triggerPressed);
        controller.inputDevice.IsPressed(InputHelpers.Button.Grip, out bool gripPressed);

        // Select object using trigger or grip
        if (selectedObject == null)
        {
            if (triggerPressed)
                TrySelectObject(Mode.Rotate);
            else if (gripPressed)
                TrySelectObject(Mode.Scale);
        }
        else if (!triggerPressed && !gripPressed)
        {
            // Deselect when both buttons are released
            selectedObject = null;
            currentMode = Mode.None;
        }
    }

    void TrySelectObject(Mode mode)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            selectedObject = hit.transform;
            currentMode = mode;
        }
    }
}
