using UnityEngine;

public class WeepingAngel : MonoBehaviour
{
    [Header("Target & Speed")]
    public Transform player;
    public float moveSpeed = 4f;
    
    [Header("Camera Setup")]
    public Camera mainCamera; // Drag your player's Main Camera here!

    private Renderer[] allRenderers;

    void Start()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
        
        // Fallback just in case you forget to assign the camera
        if (mainCamera == null) mainCamera = Camera.main; 
    }

    void Update()
    {
        if (player == null || allRenderers == null || allRenderers.Length == 0 || mainCamera == null) return;

        bool isBeingLookedAt = false;

        // 1. Get the specific camera's view (ignores portal cameras!)
        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // 2. Check if any part of the model is inside that camera's view
        foreach (Renderer part in allRenderers)
        {
            if (GeometryUtility.TestPlanesAABB(cameraPlanes, part.bounds))
            {
                isBeingLookedAt = true;
                break; 
            }
        }

        // If the Main Camera can't see it, move!
        if (!isBeingLookedAt)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; 

            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }
}