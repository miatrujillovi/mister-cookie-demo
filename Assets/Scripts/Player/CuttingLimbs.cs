using System;
using System.Collections.Generic;
using UnityEngine;

public class CuttingLimbs : MonoBehaviour
{
    public enum LimbType
    {
        Hand,
        Leg
    }

    [SerializeField] private List<GameObject> limbs;

    public static Action<LimbType> onLimbLost;

    public void CutRightHand()
    {
        CutOffLimb("RightHand");
        onLimbLost?.Invoke(LimbType.Hand);
    }

    public void CutLeftHand()
    {
        CutOffLimb("LeftHand");
        onLimbLost?.Invoke(LimbType.Hand);
    }

    public void CutRightLeg()
    {
        CutOffLimb("RightLeg");
        onLimbLost?.Invoke(LimbType.Leg);
    }

    public void CutLeftLeg()
    {
        CutOffLimb("LeftLeg");
        onLimbLost?.Invoke(LimbType.Leg);
    }

    private void CutOffLimb(string _limbName)
    {
        foreach (GameObject obj in limbs)
        {
            if (obj.name == _limbName)
            {
                DeparentObject(obj);
            }
        }
    }

    private void DeparentObject(GameObject _child)
    {
        _child.transform.SetParent(null, true);
        Rigidbody childRB = _child.AddComponent<Rigidbody>();
        BoxCollider boxCollider = _child.AddComponent<BoxCollider>();
    }
}
