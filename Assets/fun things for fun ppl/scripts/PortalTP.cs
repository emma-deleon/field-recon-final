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

    public Camera portalCamera; // Camera at the destination
    public Renderer portalScreen; // Screen to display the portal view

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
            portalScreen.material.mainTexture = portalTexture;
        }
    }

		private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Triggered by: " + other.name);

		CharacterController controller = other.GetComponentInParent<CharacterController>();

		if (controller != null)
		{
			GameObject player = controller.gameObject;

			// Get movement provider
			moveProvider = player.GetComponentInChildren<ActionBasedContinuousMoveProvider>();

			StartCoroutine(TeleportPlayer(player));
		}
	}

    private IEnumerator TeleportPlayer(GameObject player)
    {
        // Disable specified GameObjects
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
			controller.enabled = false; // IMPORTANT for XR / CC
		}

		player.transform.position = targetTeleportLocation.position;
		player.transform.rotation = targetTeleportLocation.rotation;

		if (controller != null)
		{
			controller.enabled = true;
		}

        // Wait one frame
        yield return null;

        // Re-enable objects
        foreach (var obj in objectsToDisableDuringTeleport)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Re-enable movement
        if (moveProvider != null)
            moveProvider.enabled = true;
		
		GetComponent<Collider>().enabled = false; // Prevents re-triggering
		if (portalScreen != null) portalScreen.enabled = false; // Hides the portal visuals
		if (portalCamera != null) portalCamera.enabled = false; // Stops the portal camera
    }
}