using UnityEngine;
using DG.Tweening;

public class WeightPlatformANIM : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private float maxDepressDistance = 0.5f;
    [SerializeField] private float animDuration = 0.2f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = platform.transform.localPosition;
    }

    private void OnEnable()
    {
        WeightPlatformTrigger.onPlatformReceived += SuppresingPlatform;
    }

    private void OnDisable()
    {
        WeightPlatformTrigger.onPlatformReceived -= SuppresingPlatform;
    }

    private void SuppresingPlatform(int _currentWeight, int _weightRequired, bool _doubleWeightRequired, int _secondWeightRequired)
    {
        float targetWeight = _doubleWeightRequired ? _secondWeightRequired : _weightRequired;
        float normalized = Mathf.Clamp01((float)_currentWeight / targetWeight);

        float targetY = initialPosition.y - (maxDepressDistance * normalized);

        Vector3 targetPosition = new Vector3(initialPosition.x, targetY, initialPosition.z);

        platform.transform.DOLocalMove(targetPosition, animDuration).SetEase(Ease.OutQuad);
    }
}
