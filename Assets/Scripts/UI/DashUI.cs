using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField] private Image dashFill;
    [SerializeField] private GameObject sliderContainer;

    public void Show()
    {
        sliderContainer.SetActive(true);
    }

    public void Hide()
    {
        sliderContainer.SetActive(false);
    }

    public void UpdateDash(float _normalizedValue)
    {
        dashFill.fillAmount = _normalizedValue;
    }
}
