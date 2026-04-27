using UnityEngine;
using UnityEngine.Events;

public class SimpleTriggerEvent : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If checked, this trigger will only work once.")]
    [SerializeField] private bool disableAfterUse = false;
    private bool hasBeenUsed = false;

    [Header("Trigger Events")]
    [Space(10)]

    [Tooltip("Things that will happen when a collider enters this trigger")]
    public UnityEvent onTriggerEnter;

    [Space(10)]

    [Tooltip("Things that will happen when a collider exits this trigger")]
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's already been used (if that setting is on)
        if (disableAfterUse && hasBeenUsed) return;

        if (other.CompareTag("Player"))
        {
            onTriggerEnter?.Invoke();
            hasBeenUsed = true; // Mark as used
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if it's already been used (if that setting is on)
        if (disableAfterUse && hasBeenUsed) return;

        if (other.CompareTag("Player"))
        {
            onTriggerExit?.Invoke();
        }
    }
}