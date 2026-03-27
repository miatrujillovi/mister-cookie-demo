using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WeightPlatformTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiIndicator;
    [SerializeField] private string originalText;
    [Space]
    [SerializeField] private int requiredWeight;
    public UnityEvent eventsToStart1;
    public UnityEvent eventsToStop1;
    [Space]
    [SerializeField] private bool doubleWeightRequired;
    [SerializeField] private int secondRequiredWeight;
    public UnityEvent eventsToStart2;
    public UnityEvent eventsToStop2;

    public static Action<int, int, bool, int> onPlatformReceived;

    private int currentWeight = 0;
    private LimbWeight limbWeight;
    private Dictionary<Collider, int> limbsInside = new Dictionary<Collider, int>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        limbWeight = other.GetComponent<LimbWeight>();
        if (limbWeight == null) return;

        int weight = limbWeight.GetWeight();

        limbsInside.Add(other, weight);
        currentWeight += weight;

        CheckWeight();
    }

    private void OnTriggerExit(Collider other)
    {
        if (limbsInside.ContainsKey(other))
        {
            currentWeight -= limbsInside[other];
            limbsInside.Remove(other);

            CheckWeight();
        }
    }

    private void CheckWeight()
    {
        onPlatformReceived?.Invoke(currentWeight, requiredWeight, doubleWeightRequired, secondRequiredWeight);

        if (doubleWeightRequired)
        {
            if (currentWeight >= secondRequiredWeight)
            {
                Debug.Log("Second Required weight met");
                uiIndicator.text = "Done";
                eventsToStart2?.Invoke();
            }
            else
            {
                Debug.Log("Second Required weight was NOT met");
                uiIndicator.text = originalText;
                eventsToStop2?.Invoke();
            }
        } 
        else
        {
            if (currentWeight >= requiredWeight)
            {
                Debug.Log("Required weight met");
                uiIndicator.text = "Done";
                eventsToStart1?.Invoke();
            } 
            else
            {
                Debug.Log("Required weight was NOT met");
                uiIndicator.text = originalText;
                eventsToStop1?.Invoke();   
            }
        }
    }
}
