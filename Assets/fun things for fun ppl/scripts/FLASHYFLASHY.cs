using UnityEngine;
using TMPro;

public class FLASHYFLASHY : MonoBehaviour
{
    public Light flashlight;
    public TextMeshProUGUI helpText;

    public float fadeSpeed = 0.2f;
    private bool isFading = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
            isFading = true;
        }

        if (isFading && helpText != null)
        {
            Color currentColor = helpText.color;
            
            currentColor.a -= fadeSpeed * Time.deltaTime;
            
            helpText.color = currentColor;

            if (currentColor.a <= 0)
            {
                helpText.gameObject.SetActive(false);
            }
        }
    }
}