using UnityEngine;

public class TintZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public Color zoneColor = new Color(1f, 0f, 0f, 0.5f); // Default is 50% transparent Red
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Tell the manager to fade to THIS box's specific color
            if (TintManager.instance != null)
            {
                TintManager.instance.FadeToColor(zoneColor);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Tell the manager to fade back to clear
            if (TintManager.instance != null)
            {
                TintManager.instance.ClearTint();
            }
        }
    }
}