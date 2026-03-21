using UnityEngine;

public class WeightManager : MonoBehaviour
{
    [SerializeField] private int torsoHeadWeight = 5;
    [SerializeField] private int handWeight = 2;
    [SerializeField] private int legWeight = 3;

    private int totalPlayerWeight;

    private void Start()
    {
        totalPlayerWeight = torsoHeadWeight + (handWeight * 2) + (legWeight * 2);
    }

    private void OnEnable()
    {
        CuttingLimbs.onLimbLost += WeightLost;
    }

    private void OnDisable()
    {
        CuttingLimbs.onLimbLost -= WeightLost;
    }

    private void WeightLost(CuttingLimbs.LimbType _type)
    {
        if (totalPlayerWeight == 0)
            return;

        if (_type.ToString() == "Hand")
        {
            totalPlayerWeight -= handWeight;
        } 
        else if (_type.ToString() == "Leg")
        {
            totalPlayerWeight -= legWeight;
        }

        Debug.Log("Current Player Weight: " + totalPlayerWeight);
    }
}
