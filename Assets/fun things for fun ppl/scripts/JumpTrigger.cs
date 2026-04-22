using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [Header("Setup")]
    public GameObject jumperObject; // The object that pops out
    public string playerTag = "Player";

    [Header("Physics Settings")]
    public float jumpForce = 15f;
    public Vector3 jumpDirection = new Vector3(0, 1, 0.5f); // Up and slightly forward
    public float upwardTorque = 5f; // Makes it spin a bit for realism

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Only trigger once and only if it's the player
        if (!_hasTriggered && other.CompareTag(playerTag))
        {
            _hasTriggered = true;
            PopOut();
        }
    }

    private void PopOut()
    {
        if (jumperObject == null) return;

        // 2. Ensure the object is visible/active
        jumperObject.SetActive(true);

        // 3. Get the Rigidbody (it MUST have one!)
        Rigidbody rb = jumperObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Reset velocity in case it was already moving
            rb.linearVelocity = Vector3.zero;

            // 4. Apply the 'Pop' force
            // Impulse is best for sudden explosions/jumps
            rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);

            // 5. Add a random spin (Torque) so it looks less robotic
            rb.AddTorque(Random.insideUnitSphere * upwardTorque, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("The Jumper Object needs a Rigidbody component to move with physics!");
        }
    }
}