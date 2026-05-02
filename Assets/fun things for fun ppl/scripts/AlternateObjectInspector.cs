using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class AlternateObjectInspector : MonoBehaviour
{
    [Header("Interaction Range")]
    public Transform player; 
    public float interactRange = 3f; 

    [Header("Core References")]
    public GameObject objectToInspect;
    public Transform inspectAnchor;

    [Header("UI Settings")]
    public GameObject inspectPromptUI;
    [Tooltip("Drag your 'Press ESC to Exit' Text GameObject here")]
    public GameObject exitPromptUI; // <--- NEW EXIT UI SLOT

    [Header("Player Control - FREEZE")]
    public MonoBehaviour playerLookScript;
    public MonoBehaviour playerMovementScript;

    [Header("Post-Processing - BLUR")]
    public Volume inspectBlurVolume;
    public float blurFadeSpeed = 5f;

    [Header("Inspection Settings")]
    public Key inspectKey = Key.I;
    public float rotationSpeed = 0.5f;

    private bool _isInspecting = false;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private float _targetBlurWeight = 0f;

    void Start()
    {
        if (objectToInspect == null) objectToInspect = this.gameObject;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (inspectBlurVolume != null) inspectBlurVolume.weight = 0f;
        if (inspectPromptUI != null) inspectPromptUI.SetActive(false);
        if (exitPromptUI != null) exitPromptUI.SetActive(false); // Ensure exit prompt is off at start
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null || player == null) return;

        // 1. Check distance
        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= interactRange;

        // 2. Handle Entry UI Prompt Visibility
        if (inspectPromptUI != null)
        {
            inspectPromptUI.SetActive(inRange && !_isInspecting);
        }

        // 3. Toggle Inspect Mode with [I]
        if (inRange && Keyboard.current[inspectKey].wasPressedThisFrame)
        {
            ToggleInspect();
        }
        // Exit Inspect Mode with [ESC]
        else if (_isInspecting && Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            ToggleInspect();
        }

        // 4. Force exit if player somehow moves out of range
        if (!inRange && _isInspecting)
        {
            ToggleInspect();
        }

        // 5. Handle Rotation
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

        // 6. Handle Blur
        HandleBlurTransition();
    }

    void ToggleInspect()
    {
        _isInspecting = !_isInspecting;

        if (objectToInspect != null)
        {
            if (_isInspecting)
            {
                _originalPosition = objectToInspect.transform.position;
                _originalRotation = objectToInspect.transform.rotation;
                objectToInspect.transform.position = inspectAnchor.position;
            }
            else
            {
                objectToInspect.transform.position = _originalPosition;
                objectToInspect.transform.rotation = _originalRotation;
            }
        }

        // Toggle the Exit UI on/off based on inspection state
        if (exitPromptUI != null)
        {
            exitPromptUI.SetActive(_isInspecting);
        }

        if (playerLookScript != null) playerLookScript.enabled = !_isInspecting;
        if (playerMovementScript != null) playerMovementScript.enabled = !_isInspecting;

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