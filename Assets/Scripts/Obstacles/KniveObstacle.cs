using UnityEngine;
using DG.Tweening;

public class KniveObstacle : MonoBehaviour
{
    public enum KniveType
    {
        Degollador,
        GiradorHorizontal,
        GiradorVertical
    }

    [SerializeField] private KniveType type;
    [Space]
    [Header("Degollador Characteristics")]
    [SerializeField] private float degolladorAnimDuration;
    [SerializeField] private float degolladorTimerToGoBackUp;
    [SerializeField] private float degolladorDistanceInY;
    [SerializeField] private bool makeDegolladoresGoAtDifferentSpeeds;
    [Space]
    [Header("Giradores Characteristics")]
    [SerializeField] private float giradoresAnimSpeed;
    [SerializeField] private bool makeGiradoresGoAtDifferentSpeeds;

    private void Start()
    {
        switch (type)
        {
            case KniveType.Degollador:
                DegolladorMovement();
            break;

            case KniveType.GiradorHorizontal:
                GiradorHorizontalMovement();
            break;

            case KniveType.GiradorVertical:
                GiradorVerticalMovement();
            break;

            default:
                Debug.LogError("Knive Type was not Selected");
            break;

        }
    }

    private void DegolladorMovement()
    {
        float duration = degolladorAnimDuration;

        //Optional Random Speed
        if (makeDegolladoresGoAtDifferentSpeeds)
        {
            duration *= Random.Range(0.7f, 1.3f);
        }

        float startY = transform.position.y;
        float targetY = startY - degolladorDistanceInY;

        //Starting Animation sequence
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(degolladorTimerToGoBackUp); //Timer to wait

        seq.Append(transform.DOLocalMoveY(targetY, duration).SetEase(Ease.InQuad)); //Falling down

        seq.AppendInterval(degolladorTimerToGoBackUp); //Timer to go back up

        seq.SetLoops(-1, LoopType.Yoyo); //Establish a loop
    }

    private void GiradorHorizontalMovement()
    {
        float speed = giradoresAnimSpeed;

        //Optional Random Speed
        if (makeGiradoresGoAtDifferentSpeeds)
        {
            speed *= Random.Range(0.7f, 1.3f);
        }

        transform.DORotate(new Vector3(0f, 0f, 360f), speed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void GiradorVerticalMovement()
    {
        float speed = giradoresAnimSpeed;

        //Optional Random Speed
        if (makeGiradoresGoAtDifferentSpeeds)
        {
            speed *= Random.Range(0.7f, 1.3f);
        }

        transform.DORotate(new Vector3(0f, 0f, 360f), speed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
