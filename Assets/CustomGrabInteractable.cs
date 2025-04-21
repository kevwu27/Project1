using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable
{
    private TwoHandManipulation twoHandManipulation;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Find the TwoHandManipulation component and start manipulation
        twoHandManipulation = GetComponent<TwoHandManipulation>();
        if (twoHandManipulation != null)
        {
            twoHandManipulation.StartManipulating();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Stop manipulation when deselected
        if (twoHandManipulation != null)
        {
            twoHandManipulation.StopManipulating();
        }
    }
}
