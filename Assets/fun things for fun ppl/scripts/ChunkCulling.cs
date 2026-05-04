using UnityEngine;

public class ChunkCulling : MonoBehaviour
{
    public Transform player;
    public float activeDistance = 40f;

    private Renderer[] rends;

    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        bool visible = dist < activeDistance;

        foreach (var r in rends)
            r.enabled = visible;
    }
}