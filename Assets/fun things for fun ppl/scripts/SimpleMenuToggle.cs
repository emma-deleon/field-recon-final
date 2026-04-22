using UnityEngine;

public class SimpleMenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // Drag your UI Panel here
    [SerializeField] private KeyCode toggleKey = KeyCode.P; // The "P" key

    void Update()
    {
        // Check if the player pressed the key
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            // This line flips the state: If active, make inactive. If inactive, make active.
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);

            // Handle the cursor so you can actually click things in the menu
            if (!isActive) // If we just opened it
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Optional: Time.timeScale = 0f; // Pause the game
            }
            else // If we just closed it
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // Optional: Time.timeScale = 1f; // Unpause the game
            }
        }
    }
}