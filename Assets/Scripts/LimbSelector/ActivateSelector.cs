using UnityEngine;

public class ActivateSelector : MonoBehaviour
{
    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private Selector selector;

    private bool isOpen = false;

    private void Update()
    {
        if (InputManager.Instance.Selection && !isOpen)
        {
            OpenSelectionMenu();
            isOpen = true;
        }
        else if (!InputManager.Instance.Selection && isOpen) 
        {
            CloseSelectionMenu();
            isOpen = false;
        }
    }

    private void OpenSelectionMenu()
    {
        selectionMenu.SetActive(true);
        selector.AnimateOpen();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseSelectionMenu()
    {
        selector.AnimateClose();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CancelInvoke(nameof(CloseMenu));
        Invoke(nameof(CloseMenu), 0.4f);
    }

    private void CloseMenu()
    {
        selectionMenu.SetActive(false);
    }
}
