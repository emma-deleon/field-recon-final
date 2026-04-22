using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem; 

public class ObjectInspector : MonoBehaviour
{
    [Header("Core References")]
    public GameObject objectToInspect; 
    public Transform inspectAnchor;    

    [Header("UI Settings")]
    [Tooltip("Drag your 'Press I to Inspect' Text GameObject here")]
    public GameObject inspectPromptUI; 

    [Header("Player Control - FREEZE")]
    public MonoBehaviour playerLookScript; 
    
    [Header("Post-Processing - BLUR")]
    public Volume inspectBlurVolume; 
    public float blurFadeSpeed = 5f;

    [Header("Inspection Settings")]
    public Key inspectKey = Key.I; 
    public float rotationSpeed = 0.5f; 

    private bool _isInspecting = false;
    private Quaternion _originalRotation;
    private float _targetBlurWeight = 0f;

    void Start()
    {
        if (objectToInspect != null)
        {
            _originalRotation = objectToInspect.transform.localRotation;
            objectToInspect.SetActive(false);
        }

        if (inspectBlurVolume != null) inspectBlurVolume.weight = 0f;
        
        // Make sure the prompt is visible when the game starts
        if (inspectPromptUI != null) inspectPromptUI.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // 1. Toggle mode
        if (Keyboard.current[inspectKey].wasPressedThisFrame)
        {
            ToggleInspect();
        }

        // 2. Handle Rotation
        if (_isInspecting && objectToInspect != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                float mouseX = mouseDelta.x * rotationSpeed;
                float mouseY = mouseDelta.y * rotationSpeed;

                objectToInspect.transform.Rotate(Camera.main.transform.up, -mouseX, Space.World);
                objectToInspect.transform.Rotate(Camera.main.transform.right, mouseY, Space.World);
            }
        }

        // 3. Handle Blur
        HandleBlurTransition();
    }

    void ToggleInspect()
    {
        _isInspecting = !_isInspecting;

        // UI on until button pressed
        if (inspectPromptUI != null)
        {
            inspectPromptUI.SetActive(false);
        }

        if (objectToInspect != null)
        {
            if (_isInspecting)
            {
                objectToInspect.transform.position = inspectAnchor.position;
                objectToInspect.SetActive(true);
            }
            else
            {
                objectToInspect.transform.localRotation = _originalRotation;
                objectToInspect.SetActive(false);
            }
        }

        if (playerLookScript != null)
        {
            playerLookScript.enabled = !_isInspecting;
        }

        _targetBlurWeight = _isInspecting ? 1f : 0f;
    }

    void HandleBlurTransition()
    {
        if (inspectBlurVolume == null) return;

        if (!Mathf.Approximately(inspectBlurVolume.weight, _targetBlurWeight))
        {
            inspectBlurVolume.weight = Mathf.MoveTowards(inspectBlurVolume.weight, _targetBlurWeight, Time.deltaTime * blurFadeSpeed);
        }
    }
}