using UnityEngine;

public class scrollscrollscroll : MonoBehaviour
{
    public Vector2 offsetAmount = Vector2.right;
    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 newOffset = mat.GetTextureOffset("_BaseMap") + offsetAmount * Time.deltaTime;
        mat.SetTextureOffset("_BaseMap", newOffset);
    }
}