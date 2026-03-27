using UnityEngine;

public class LimbRegenerator : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private CuttingLimbs cuttingLimbs;

    private void OnEnable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager.Instance es null en LimbRegenerator");
            return;
        }
        InputManager.Instance.OnInteract += TryRegenerate;
        Debug.Log("LimbRegenerator suscrito a OnInteract");
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract -= TryRegenerate;
    }

    private void TryRegenerate()
    {
        Debug.Log("TryRegenerate llamado");

        if (player == null) { Debug.LogError("player no asignado"); return; }
        if (cuttingLimbs == null) { Debug.LogError("cuttingLimbs no asignado"); return; }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distancia al horno: " + distance + " | Rango: " + interactRange);

        if (distance <= interactRange)
            cuttingLimbs.RegenerateLimbs();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}