using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class AlternateObjectInspector : MonoBehaviour
{
    // These static variables are shared by ALL books/keys in the scene
    public static AlternateObjectInspector CurrentActiveInspector = null;
    public static AlternateObjectInspector PromptOwner = null;

    [Header("1. Distance Settings")]
    public Transform player; 
    public float interactRange = 2.5f; 

    [Header("2. The Floating Model")]
    [Tooltip("Drag the specific floating model for THIS object here.")]
    public GameObject myFloatingModel; 

    [Header("3. Shared UI & Effects")]
    public GameObject inspectPromptUI; 
    public GameObject exitPromptUI;    
    public CanvasGroup backgroundDimmer;
    public Volume inspectBlurVolume;
    public float transitionSpeed = 8f;

    [Header("4. Player Control")]
    public MonoBehaviour playerLookScript;
    public MonoBehaviour playerMovementScript;

    [Header("5. Settings")]
    public float rotationSpeed = 0.5f;
    public string inspectLayerName = "Inspect";

    private bool _isInspecting = false;
    private Quaternion _originalRotation;
    private MeshRenderer _myTableMesh;

    void Start()
    {
        _myTableMesh = GetComponent<MeshRenderer>();

        if (myFloatingModel != null)
        {
            _originalRotation = myFloatingModel.transform.localRotation;
            myFloatingModel.SetActive(false);
            
            int layer = LayerMask.NameToLayer(inspectLayerName);
            if (layer != -1) myFloatingModel.layer = layer;
        }

        // Initially hide UI
        if (inspectPromptUI != null) inspectPromptUI.SetActive(false);
        if (exitPromptUI != null) exitPromptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRange = dist <= interactRange;

        // --- PROMPT LOGIC (The fix for multiple objects) ---
        if (!_isInspecting && CurrentActiveInspector == null)
        {
            if (inRange)
            {
                // If no one is using the prompt, I'll take it
                if (PromptOwner == null)
                {
                    PromptOwner = this;
                    if (inspectPromptUI != null) inspectPromptUI.SetActive(true);
                }
                
                // If I am the one owning the prompt, check for input
                if (PromptOwner == this && Keyboard.current.iKey.wasPressedThisFrame)
                {
                    ToggleInspect(true);
                }
            }
            else
            {
                // ONLY turn off the UI if I am the one who turned it on
                if (PromptOwner == this)
                {
                    PromptOwner = null;
                    if (inspectPromptUI != null) inspectPromptUI.SetActive(false);
                }
            }
        }

        // --- INSPECTION LOGIC ---
        if (_isInspecting)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame || !inRange)
            {
                ToggleInspect(false);
            }

            if (myFloatingModel != null && Mouse.current.leftButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                myFloatingModel.transform.Rotate(Camera.main.transform.up, -delta.x * rotationSpeed, Space.World);
                myFloatingModel.transform.Rotate(Camera.main.transform.right, delta.y * rotationSpeed, Space.World);
            }
        }

        // Only handle transitions if I'm the one being looked at
        if (CurrentActiveInspector == this || CurrentActiveInspector == null)
        {
            HandleTransitions();
        }
    }

    void ToggleInspect(bool shouldInspect)
    {
        _isInspecting = shouldInspect;
        
        if (shouldInspect)
        {
            CurrentActiveInspector = this;
            PromptOwner = null; // Clear prompt owner so it doesn't stay on screen
            if (inspectPromptUI != null) inspectPromptUI.SetActive(false);
            if (exitPromptUI != null) exitPromptUI.SetActive(true);

            if (_myTableMesh != null) _myTableMesh.enabled = false;
            if (myFloatingModel != null)
            {
                myFloatingModel.transform.localRotation = _originalRotation;
                myFloatingModel.SetActive(true);
            }
        }
        else
        {
            CurrentActiveInspector = null;
            if (exitPromptUI != null) exitPromptUI.SetActive(false);

            if (_myTableMesh != null) _myTableMesh.enabled = true;
            if (myFloatingModel != null) myFloatingModel.SetActive(false);
        }

        if (playerLookScript != null) playerLookScript.enabled = !shouldInspect;
        if (playerMovementScript != null) playerMovementScript.enabled = !shouldInspect;
    }

    void HandleTransitions()
    {
        float target = _isInspecting ? 1f : 0f;
        if (inspectBlurVolume != null)
            inspectBlurVolume.weight = Mathf.MoveTowards(inspectBlurVolume.weight, target, Time.deltaTime * transitionSpeed);
        if (backgroundDimmer != null)
            backgroundDimmer.alpha = Mathf.MoveTowards(backgroundDimmer.alpha, target, Time.deltaTime * transitionSpeed);
    }
}