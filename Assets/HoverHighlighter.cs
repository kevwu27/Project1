using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverHighlighter : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    public void OnHoverEnter()
    {
        meshRenderer.material = highlightMaterial;
    }

    public void OnHoverExit()
    {
        meshRenderer.material = originalMaterial;
    }
}
