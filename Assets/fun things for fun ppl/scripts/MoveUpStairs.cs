using UnityEngine;

public class MoveUpStairs : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the escalator moves the player.")]
    public float transportSpeed = 5.0f;
    
    [Tooltip("The direction of travel relative to the trigger box. (0,0,1) is 'Forward'")]
    public Vector3 localDirection = Vector3.forward;

    [Header("if its not working")]
    [Tooltip("If checked, it skips the Character Controller and moves the Player transform directly.")]
    public bool forceTransformMove = false;


    private void OnTriggerStay(Collider other)
    {
        // 1. ABSOLUTE LOG: If ANYTHING touches this, print it.
        //Debug.Log("PHYSICS CONTACT: " + other.name);

        // 2. CHECK TAG:
        if (other.CompareTag("Player"))
        {
            //Debug.Log("TAG MATCH: Player found!");
            
            CharacterController controller = other.GetComponentInParent<CharacterController>();
            if (controller != null)
            {
                //Debug.Log("CONTROLLER MATCH: Moving player now.");
                Vector3 worldDir = transform.TransformDirection(localDirection);
                controller.Move(worldDir * transportSpeed * Time.deltaTime);
            }
            else
            {
                //Debug.Log("MISSING: No CharacterController found on " + other.name);
            }
        }
    }
}