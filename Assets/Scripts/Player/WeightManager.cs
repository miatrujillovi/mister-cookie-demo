using UnityEngine;

public class WeightManager : MonoBehaviour
{
    public static WeightManager instance;

    [SerializeField] private int torsoHeadWeight = 5;
    [SerializeField] private int handWeight = 2;
    [SerializeField] private int legWeight = 3;

    private int totalPlayerWeight;
    private int currentWeight;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        totalPlayerWeight = torsoHeadWeight + (handWeight * 2) + (legWeight * 2);
        currentWeight = totalPlayerWeight;
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
        if (currentWeight == 0)
            return;

        if (_type.ToString() == "Hand")
        {
            currentWeight -= handWeight;
        } 
        else if (_type.ToString() == "Leg")
        {
            currentWeight -= legWeight;
        }

        Debug.Log("Current Player Weight: " + currentWeight);
    }

    private void GainWeight()
    {
        currentWeight = totalPlayerWeight;
    }

    //GETTER
    public int GetWeight()
    {
        return currentWeight;
    }
}
