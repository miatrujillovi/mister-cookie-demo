using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;

    private GameObject playerBody;

    private void OnTriggerEnter(Collider other)
    {
        Transform[] children = other.GetComponentsInChildren<Transform>();

        foreach (Transform t in children)
        {
            if (t.CompareTag("PlayerBody"))
            {
                playerBody = t.gameObject;

                gameOverCanvas.SetActive(true);

                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;
            }
        }
    }
}
