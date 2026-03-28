using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class LimbRegenerator : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private CuttingLimbs cuttingLimbs;
    [SerializeField] private Selector selector;
    [SerializeField] private TextMeshProUGUI interactionTXT;
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool wasInRange = false;
    private Coroutine feedbackCoroutine;
    private Color originalColor;

    private void Start()
    {
        originalColor = interactionTXT.color;
    }

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
        AudioManager.Instance.PlayHornoRegenerate();
        if (player == null) { Debug.LogError("player no asignado"); return; }
        if (cuttingLimbs == null) { Debug.LogError("cuttingLimbs no asignado"); return; }

        float distance = Vector3.Distance(transform.position, player.position);

        if (cuttingLimbs.currentLimbs == null || cuttingLimbs.currentLimbs.Count == 4)
        {
            if (feedbackCoroutine != null)
                StopCoroutine(feedbackCoroutine);

            feedbackCoroutine = StartCoroutine(FlashRed());
            return;
        }

        // Has limbs
        //cuttingLimbs.RegenerateLimbs();
        StartCoroutine(RegenerationSequence());
    }

    private IEnumerator RegenerationSequence()
    {
        // Fade out screen
        yield return StartCoroutine(Fade(0f, 1f));

        // Do the regeneration while screen is black
        cuttingLimbs.RegenerateLimbs();
        selector.EnableButtons();

        yield return new WaitForSeconds(0.3f); // small pause

        // Fade in screen
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, t / fadeDuration);
            fadePanel.alpha = alpha;
            yield return null;
        }

        fadePanel.alpha = end;
    }

    private IEnumerator FlashRed()
    {
        interactionTXT.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        interactionTXT.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= interactRange;

        if (inRange != wasInRange)
        {
            interactionTXT.gameObject.SetActive(inRange);
            wasInRange = inRange;
        }
    }
}