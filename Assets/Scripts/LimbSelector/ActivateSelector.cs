using UnityEngine;
using MoreMountains.Feedbacks;

public class ActivateSelector : MonoBehaviour
{
    [SerializeField] private GameObject selectionMenu;

    private void Update()
    {
        if (InputManager.Instance.Selection)
        {
            OpenSelectionMenu();
        }
        else
        {
            CloseSelectionMenu();
        }
    }

    private void OpenSelectionMenu()
    {
        selectionMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseSelectionMenu()
    {
        selectionMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
