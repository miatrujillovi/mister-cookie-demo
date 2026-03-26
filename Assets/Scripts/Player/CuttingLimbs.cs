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
    [SerializeField] private LimbLauncher limbLauncher;

    [HideInInspector] public List<string> currentLimbs;

    public static Action<LimbType> onLimbLost;

    private bool selection;

    private void Start()
    {
        foreach (var _limb in limbs)
        {
            currentLimbs.Add(_limb.name);
        }

        currentLimbs.Remove("Head");
        currentLimbs.Remove("Torso");
    }

    public void CutRightHand(bool _selection)
    {
        selection = _selection;

        CutOffLimb("RightHand");
        onLimbLost?.Invoke(LimbType.Hand);
    }

    public void CutLeftHand(bool _selection)
    {
        selection = _selection;

        CutOffLimb("LeftHand");
        onLimbLost?.Invoke(LimbType.Hand);
    }

    public void CutRightLeg(bool _selection)
    {
        selection = _selection;

        CutOffLimb("RightLeg");
        onLimbLost?.Invoke(LimbType.Leg);
    }

    public void CutLeftLeg(bool _selection)
    {
        selection = _selection;

        CutOffLimb("LeftLeg");
        onLimbLost?.Invoke(LimbType.Leg);
    }

    private void CutOffLimb(string _limbName)
    {
        foreach (GameObject obj in limbs)
        {
            if (obj.name == _limbName)
            {
                currentLimbs.Remove(obj.name);
                DeparentObject(obj);
            }
        }
    }

    private void DeparentObject(GameObject _child)
    {
        _child.transform.SetParent(null, true);
        Rigidbody childRB = _child.AddComponent<Rigidbody>();
        //BoxCollider boxCollider = _child.AddComponent<BoxCollider>();

        InputManager.Instance.DisableSelection();

        if (selection)
        {
            limbLauncher.SetSelectedLimb(_child);
        }
    }
}
