using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    public static LevelRestart Instance;

    [SerializeField] private Transform startingPosition;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RestartingLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        /*player.transform.position = startingPosition.position;

        gameOverScreen.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/

        //Call function to get all limbs back
    }
}
