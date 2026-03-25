using UnityEngine;

public class WeightTrigger : MonoBehaviour
{
    [SerializeField, Range(1, 15)] private int allowedWeight;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && WeightManager.instance.GetWeight() <= allowedWeight)
        {
            rb.mass = 0.01f;
            //Debug.Log("The current mass of the object is: " + rb.mass);
        }
    }
}
