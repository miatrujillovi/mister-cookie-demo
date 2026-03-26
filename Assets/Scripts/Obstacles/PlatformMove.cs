using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private float journeyLength;
    private float startTime;
    private bool waiting = false;
    private bool goingToB = true;
    private Vector3 startPos;
    private Vector3 targetPos;

    private void Start()
    {
        transform.position = pointA.position;
        startPos = pointA.position;
        targetPos = pointB.position;
        journeyLength = Vector3.Distance(startPos, targetPos);
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (waiting) return;
        if (journeyLength <= 0f) return;

        float distanceCovered = (Time.fixedTime - startTime) * speed;
        float fractionOfJourney = Mathf.Clamp01(distanceCovered / journeyLength);
        float curvedFraction = movementCurve.Evaluate(fractionOfJourney);
        transform.position = Vector3.Lerp(startPos, targetPos, curvedFraction);

        if (fractionOfJourney >= 1f)
        {
            waiting = true;
            Invoke(nameof(SwitchDirection), waitTime);
        }
    }

    private void SwitchDirection()
    {
        goingToB = !goingToB;
        startPos = transform.position;
        targetPos = goingToB ? pointB.position : pointA.position;
        journeyLength = Vector3.Distance(startPos, targetPos);
        startTime = Time.fixedTime; 
        waiting = false;
    }
}
