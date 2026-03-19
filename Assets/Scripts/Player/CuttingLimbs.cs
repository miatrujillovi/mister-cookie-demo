using System.Collections.Generic;
using UnityEngine;

public class CuttingLimbs : MonoBehaviour
{
    [SerializeField] private List<GameObject> limbs;

    public void CutRightHand()
    {
        CutOffLimb("RightHand");
    }

    public void CutLeftHand()
    {
        CutOffLimb("LeftHand");
    }

    public void CutRightLeg()
    {
        CutOffLimb("RightLeg");
    }

    public void CutLeftLeg()
    {
        CutOffLimb("LeftLeg");
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
