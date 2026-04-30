using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalTP : MonoBehaviour
{
    public Transform targetTeleportLocation;
    public GameObject effectPrefab;
    public AudioClip teleportSound;
    private AudioSource audioSource;

    private ActionBasedContinuousMoveProvider moveProvider;

    public GameObject[] objectsToDisableDuringTeleport;

    public GameObject shardsToDisable;

    public Camera portalCamera; 
    public Renderer portalScreen; 
	
	public Material destinationSkybox;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set up the portal camera render texture
        if (portalCamera != null && portalScreen != null)
        {
            RenderTexture portalTexture = new RenderTexture(Screen.width, Screen.height, 24);
            portalCamera.targetTexture = portalTexture;
            portalScreen.material = new Material(portalScreen.material); // important
			portalScreen.material.SetTexture("_PortalTex", portalTexture);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController controller = other.GetComponentInParent<CharacterController>();

        if (controller != null)
        {
            GameObject player = controller.gameObject;

            moveProvider = player.GetComponentInChildren<ActionBasedContinuousMoveProvider>();

            StartCoroutine(TeleportPlayer(player));
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        // Disable other objects (your existing system)
        foreach (var obj in objectsToDisableDuringTeleport)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Disable movement
        if (moveProvider != null)
            moveProvider.enabled = false;

        // Play effect
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, player.transform.position, Quaternion.identity);
        }

        // Play sound
        if (teleportSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }

        // Teleport player
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.position = targetTeleportLocation.position;
		
		Vector3 targetRotation = targetTeleportLocation.eulerAngles;
		player.transform.rotation = Quaternion.Euler(0, targetRotation.y, 0);
		
		if (destinationSkybox != null)
        {
            RenderSettings.skybox = destinationSkybox;
        }

        if (controller != null)
        {
            controller.enabled = true;
        }

        // Wait one frame
        yield return null;

        // Re-enable other objects
        foreach (var obj in objectsToDisableDuringTeleport)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Re-enable movement
        if (moveProvider != null)
            moveProvider.enabled = true;

        GetComponent<Collider>().enabled = false;
        if (portalScreen != null) portalScreen.enabled = false;
        if (portalCamera != null) portalCamera.enabled = false;

        if (shardsToDisable != null)
        {
            shardsToDisable.SetActive(false);
        }
    }
}