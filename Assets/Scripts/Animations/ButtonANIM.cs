using UnityEngine;
using DG.Tweening;

public class ButtonANIM : MonoBehaviour
{
    public enum Axes
    {
        X,
        Y,
        Z
    }

    [SerializeField] private GameObject button;
    [SerializeField] private float maxDepressDistance = 0.5f;
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private Axes buttonAxis;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = button.transform.localPosition;
    }

    public void PressingButton()
    {
        button.transform.DOKill();

        Vector3 targetPos = initialPosition;

        switch (buttonAxis)
        {
            case Axes.X:
                targetPos.x -= maxDepressDistance;
            break; 

            case Axes.Y:
                targetPos.y -= maxDepressDistance;
            break;

            case Axes.Z:
                targetPos.z -= maxDepressDistance; 
            break;
        }

        button.transform.DOLocalMove(targetPos, animDuration).SetEase(Ease.OutQuad);
    }
}
