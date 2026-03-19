using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForceUI : MonoBehaviour
{
    [SerializeField] private Slider forceSlider;
    [SerializeField] private TextMeshProUGUI forceText;
    [SerializeField] private Image fillImage;

    [SerializeField] private Color lowForceColor = Color.green;
    [SerializeField] private Color highForceColor = Color.red;

    public void UpdateForce(float current, float min, float max)
    {
        float normalized = Mathf.InverseLerp(min, max, current);
        forceSlider.value = normalized;
        forceText.text = $"Fuerza: {Mathf.RoundToInt(current)}";
        fillImage.color = Color.Lerp(lowForceColor, highForceColor, normalized);
    }
}
