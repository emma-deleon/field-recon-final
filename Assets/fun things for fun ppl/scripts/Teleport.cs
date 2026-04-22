using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destination;
    public string playerTag = "Player";

    [Header("Light Settings (Choose One Method)")]
    [Tooltip("Method A: Change the color of this light")]
    public Light targetLight; 
    public Color newLightColor = Color.white;

    [Tooltip("Method B: Swap these two light objects")]
    public GameObject lightToDeactivate;
    public GameObject lightToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TeleportAndChangeEnvironment(other.gameObject);
        }
    }

    private void TeleportAndChangeEnvironment(GameObject player)
    {
        // 1. Move Player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        // 2. Change Lights
        // Method A: Change Color
        if (targetLight != null)
        {
            targetLight.color = newLightColor;
        }

        // Method B: Swap Active State
        if (lightToDeactivate != null) lightToDeactivate.SetActive(false);
        if (lightToActivate != null) lightToActivate.SetActive(true);

        //Debug.Log("Teleport complete: Environment updated.");
    }
}