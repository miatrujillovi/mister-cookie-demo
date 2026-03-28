using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingLimbs : MonoBehaviour
{
    public enum LimbType
    {
        Hand,
        Leg
    }

    [SerializeField] private List<GameObject> limbs;
    [SerializeField] private LimbLauncher limbLauncher;
    [SerializeField] private Transform player;

    [HideInInspector] public List<string> currentLimbs;

    public static Action<LimbType> onLimbLost;

    private bool selection;

    // Guardar estado original de cada extremidad
    private Dictionary<string, Transform> originalParents = new Dictionary<string, Transform>();
    private Dictionary<string, Vector3> originalLocalPositions = new Dictionary<string, Vector3>();
    private Dictionary<string, Quaternion> originalLocalRotations = new Dictionary<string, Quaternion>();
    private List<GameObject> cutLimbs = new List<GameObject>();

    [SerializeField] private List<Button> limbButtons;

    private void Awake()
    {
        // Guarda posicion original antes de que cualquier cosa se mueva
        foreach (GameObject limb in limbs)
        {
            originalParents[limb.name] = limb.transform.parent;
            originalLocalPositions[limb.name] = limb.transform.localPosition;
            originalLocalRotations[limb.name] = limb.transform.localRotation;
        }
    }

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
        cutLimbs.Add(_child);
        //BoxCollider boxCollider = _child.AddComponent<BoxCollider>();

        InputManager.Instance.DisableSelection();

        if (selection)
        {
            limbLauncher.SetSelectedLimb(_child);
        }
    }

    public void RegenerateLimbs()
    {
        // Desactiva las extremidades lanzadas en lugar de destruirlas
        foreach (GameObject limb in cutLimbs)
        {
            if (limb != null)
                limb.SetActive(false);
        }
        cutLimbs.Clear();

        // Restaura cada extremidad a su estado original
        foreach (GameObject limb in limbs)
        {
            if (limb == null) continue;

            Rigidbody rb = limb.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            limb.transform.SetParent(originalParents[limb.name]);
            limb.transform.localPosition = originalLocalPositions[limb.name];
            limb.transform.localRotation = originalLocalRotations[limb.name];
            limb.SetActive(true);
        }

        currentLimbs.Clear();
        foreach (GameObject limb in limbs)
        {
            if (limb == null) continue;
            currentLimbs.Add(limb.name);
        }
        currentLimbs.Remove("Head");
        currentLimbs.Remove("Torso");

        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3f, player.transform.position.z);

        foreach (Button btn in limbButtons)
        {
            if (btn != null)
                btn.enabled = true;
        }

        limbLauncher.ResetLauncher();
    }
}
