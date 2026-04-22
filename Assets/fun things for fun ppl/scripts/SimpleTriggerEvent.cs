using UnityEngine;
using UnityEngine.Events;

public class SimpleTriggerEvent : MonoBehaviour
{
    [Header("Trigger Events")]
    [Space(10)]

    [Tooltip("Things that will happen when a collider enters this trigger")]
    public UnityEvent onTriggerEnter;

    [Space(10)]

    [Tooltip("Things that will happen when a collider exits this trigger")]
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
		//Debug.Log("Detected: " + other.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player detected! Invoking events...");
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExit?.Invoke();
        }
    }
}
