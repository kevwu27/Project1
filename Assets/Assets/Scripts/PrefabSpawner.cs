using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class VRPrefabSpawner : MonoBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;
    public GameObject prefabC;
    public GameObject prefabD;
    public XRRayInteractor rightRayInteractor;

    public Transform left;
    public Transform right;

    public InputActionProperty buttonAAction;
    public InputActionProperty buttonBAction;
    public InputActionProperty buttonXAction;
    public InputActionProperty buttonYAction;

    public float spawnDistance = 10.5f;

    void Update()
    {
        
        if (rightRayInteractor.hasSelection) return;
        
        if (buttonAAction.action.WasPressedThisFrame())
        {
            SpawnAndSelect(prefabA);
        }
        else if (buttonBAction.action.WasPressedThisFrame())
        {
            SpawnAndSelect(prefabB);
        }
        else if (buttonXAction.action.WasPressedThisFrame())
        {
            SpawnAndSelect(prefabC);
        }
        else if (buttonYAction.action.WasPressedThisFrame())
        {
            SpawnAndSelect(prefabD);
        }
    }

    void SpawnAndSelect(GameObject prefab)
    {
        Transform rayOrigin = rightRayInteractor.transform;
        Vector3 spawnPosition = rayOrigin.position + rayOrigin.forward * spawnDistance;
        Debug.Log(spawnPosition);
        GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);

        spawned.transform.rotation = Quaternion.LookRotation(rayOrigin.forward);

        StartCoroutine(DelayedSelect(spawned));
    }

    System.Collections.IEnumerator DelayedSelect(GameObject spawned)
    {
        yield return null; // Wait one frame

        if (spawned.TryGetComponent<TwoHandManipulation>(out var behavior))
        {
            behavior.leftController = left;
            behavior.rightController = right;
        }

        if (spawned.TryGetComponent<IXRSelectInteractable>(out var interactable))
        {
            var manager = rightRayInteractor.interactionManager;
            manager.SelectEnter(rightRayInteractor, interactable);
        }
    }

}
