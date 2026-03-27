using UnityEngine;

public class HandShadow : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] private float heightOffset = 0.05f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private float minScale = 0.3f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float animationSpeed = 1.5f;

    [Header("Transparency")]
    [SerializeField] private float minAlpha = 0.1f;
    [SerializeField] private float maxAlpha = 0.6f;

    [Header("Random Lapses")]
    [SerializeField] private float minInterval = 1f; // tiempo minimo entre lapsos
    [SerializeField] private float maxInterval = 3f; // tiempo maximo entre lapsos

    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private float currentInterval;
    private bool isAnimating = false;
    private float animationTimer = 0f;
    [SerializeField] private float animationDuration = 1.5f; // duracion de cada lapso
    private float fixedY;

    private void Start()
    {
    spriteRenderer = GetComponent<SpriteRenderer>();
    fixedY = transform.position.y;

    transform.localScale = Vector3.one * minScale;
    SetAlpha(minAlpha);

    currentInterval = Random.Range(minInterval, maxInterval);
}

    private void Update()
    {
        // Proyecta en el suelo
        if (Physics.Raycast(transform.parent.position, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            Vector3 newPos = hit.point + Vector3.up * heightOffset;
            newPos.y = fixedY; // siempre la misma Y
            transform.position = newPos;
        }

        transform.rotation = Quaternion.Euler(90f, 0f, -90f);

        // Espera el intervalo para animar
        if (!isAnimating)
        {
            timer += Time.deltaTime;
            if (timer >= currentInterval)
            {
                timer = 0f;
                isAnimating = true;
                animationTimer = 0f;
                currentInterval = Random.Range(minInterval, maxInterval);
            }
        }
        else
        {
            animationTimer += Time.deltaTime;
            float progress = animationTimer / animationDuration;

            // Sin wave que va de 0 a 1 y vuelve a 0 — aparece y desaparece
            float wave = Mathf.Sin(progress * Mathf.PI);

            // Escala entre min y max
            float scale = Mathf.Lerp(minScale, maxScale, wave);
            transform.localScale = Vector3.one * scale;

            // Alpha entre min y max
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, wave);
            SetAlpha(alpha);

            if (animationTimer >= animationDuration)
            {
                isAnimating = false;
                transform.localScale = Vector3.one * minScale;
                SetAlpha(minAlpha);
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
