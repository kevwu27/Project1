using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpawnItem : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float cooldownTime = 5f;

    private Button button;
    private TMP_Text buttonText;
    private Image buttonImage;

    private string originalLabel;
    private Color originalColor;
    public Transform leftController;
    public Transform rightController;


    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TMP_Text>();

        TwoHandManipulation manipulator = prefabToSpawn.GetComponent<TwoHandManipulation>();
        if (manipulator != null)
        {
            manipulator.leftController = leftController;
            manipulator.rightController = rightController;
        }

        if (buttonText != null)
            originalLabel = buttonText.text;

        if (buttonImage != null)
            originalColor = buttonImage.color;
    }

    public void Spawn()
    {
        if (!button.interactable) return;

        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        float remainingTime = cooldownTime;

        // Disable button
        button.interactable = false;

        // Change color to red
        if (buttonImage != null)
            buttonImage.color = Color.red;

        while (remainingTime > 0)
        {
            if (buttonText != null)
                buttonText.text = $"{originalLabel} ({Mathf.CeilToInt(remainingTime)})";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Reset visuals
        if (buttonText != null)
            buttonText.text = originalLabel;

        if (buttonImage != null)
            buttonImage.color = originalColor;

        button.interactable = true;
    }
}
