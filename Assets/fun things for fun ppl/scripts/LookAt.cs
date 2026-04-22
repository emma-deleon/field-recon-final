using UnityEngine;

public class LookAt : MonoBehaviour
{
    [Header("Targeting")]
    public string playerTag = "Player";
    private Transform _player;

    [Header("Look Settings")]
    public float turnSpeed = 5f; // Higher = faster snapping, Lower = creepy slow turn
    public bool isQuad = true;   // Keep true to fix the invisible Quad issue
    public bool keepUpright = true; // Keeps the object from tilting up/down

    void Start()
    {
        // Automatically finds the player so you don't have to drag anything in
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("LookAt Script: Couldn't find anything tagged 'Player'!");
        }
    }

    void Update()
    {
        if (_player == null) return;

        // 1. Figure out which way the player is
        Vector3 direction = _player.position - transform.position;

        // 2. Flip the direction if it's a Quad (so the image faces you)
        if (isQuad)
        {
            direction = -direction;
        }

        // 3. Lock the Y axis so it doesn't tilt into the floor
        if (keepUpright)
        {
            direction.y = 0;
        }

        // 4. Smoothly rotate to face the player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}