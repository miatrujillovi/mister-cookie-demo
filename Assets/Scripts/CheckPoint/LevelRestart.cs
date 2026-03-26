using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    public static LevelRestart Instance;

    [SerializeField] private Transform startingPosition;
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
        player.transform.position = startingPosition.position;
        //Call function to get all limbs back
    }
}
