using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float floatSpeed = 1f;      // How fast it moves up/down
    public float floatHeight = 0.05f;  // How far it moves up/down

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = originalPosition + new Vector3(0, offsetY, 0);
    }
}
