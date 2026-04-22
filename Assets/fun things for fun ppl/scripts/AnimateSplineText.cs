using UnityEngine;

public class AnimateSplineText : MonoBehaviour 
{
    public float scrollSpeed = 0.2f;
    private TMPSplineText _splineText;

    void Start() => _splineText = GetComponent<TMPSplineText>();

    void Update() 
    {
        // Continuously increase the offset
        _splineText.startOffset += Time.deltaTime * scrollSpeed;

        // Keep the offset between 0 and 1 so the slider doesn't grow to infinity
        if (_splineText.startOffset > 1f) _splineText.startOffset -= 1f;
        if (_splineText.startOffset < 0f) _splineText.startOffset += 1f;
    }
}