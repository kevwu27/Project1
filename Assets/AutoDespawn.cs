using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public float lifespan = 5.0f;

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
