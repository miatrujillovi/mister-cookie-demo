using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public enum Scenes
    {
        MainMenu,
        Tutorial,
        NivelUno,
        NivelDos,
        NivelTres
    }

    [SerializeField] private ChangeScenes changeScenes;
    [SerializeField] private Scenes sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        changeScenes.ChangeScene(sceneName.ToString());
    }
}
