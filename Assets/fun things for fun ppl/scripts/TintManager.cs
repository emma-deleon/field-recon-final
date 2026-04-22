using UnityEngine;
using UnityEngine.UI;

public class TintManager : MonoBehaviour
{
    // This makes it a "Singleton" so other scripts can instantly find it
    public static TintManager instance; 
    
    public float fadeSpeed = 3f;

    private Image _tintImage;
    private Color _targetColor = new Color(0, 0, 0, 0); // Starts totally clear

    void Awake()
    {
        instance = this;
        _tintImage = GetComponent<Image>();
        _tintImage.color = _targetColor;
    }

    // Other scripts will call this to change the color
    public void FadeToColor(Color newColor)
    {
        _targetColor = newColor;
    }

    // Other scripts will call this to clear the screen
    public void ClearTint()
    {
        // Keeps the current RGB color, but drops the Alpha (transparency) to 0
        _targetColor = new Color(_targetColor.r, _targetColor.g, _targetColor.b, 0f);
    }

    void Update()
    {
        if (_tintImage != null)
        {
            // Smoothly shift the current color toward the target color
            _tintImage.color = Color.Lerp(_tintImage.color, _targetColor, fadeSpeed * Time.deltaTime);
        }
    }
}
