using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform initialPosition, targetPosition;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private bool buttonActivated = false;

    private Rigidbody rb;
    private Vector3 currentTarget;
    private bool goingToTarget = true;
    private bool isWaiting = false;
    private bool stop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.position = initialPosition.position;
        currentTarget = targetPosition.position;
    }

    private void FixedUpdate()
    {
        if (stop) return;

        if (isWaiting) return;

        if (buttonActivated) return;

        Vector3 newPosition = Vector3.MoveTowards(rb.position,currentTarget,speed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        if (Vector3.Distance(rb.position, currentTarget) < 0.05f)
        {
            StartCoroutine(SwitchTarget());
        }
    }

    private IEnumerator SwitchTarget()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        goingToTarget = !goingToTarget;
        currentTarget = goingToTarget ? targetPosition.position : initialPosition.position;

        isWaiting = false;
    }

    public void ActivatePlatform()
    {
        buttonActivated = false;
        stop = false;
    }

    public void StopPlatformMovement()
    {
        stop = true;
    }
}