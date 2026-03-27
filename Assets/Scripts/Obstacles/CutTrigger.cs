using System.Collections;
using UnityEngine;

public class CutTrigger : MonoBehaviour
{
    [SerializeField] private CuttingLimbs cuttingLimbs;

    private string limbToCut;
    private bool hasTrigerred = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.transform.root != cuttingLimbs.transform.root) return;

        if (hasTrigerred) return;

        hasTrigerred = true;

        SelectLimbToCut();
        StartCoroutine(WaitToTriggerAgain());
    }

    private IEnumerator WaitToTriggerAgain()
    {
        yield return new WaitForSeconds(1.5f);
        hasTrigerred = false;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        hasTrigerred = false;
    }*/

    private void SelectLimbToCut()
    {
        if (cuttingLimbs.currentLimbs == null) return;

        int limbCounter = cuttingLimbs.currentLimbs.Count;

        if (limbCounter == 0)
        {
            Debug.Log("Player has died");
            LevelRestart.Instance.RestartingLevel();
            return;
        }

        int randomIndex = Random.Range(0, limbCounter);

        limbToCut = cuttingLimbs.currentLimbs[randomIndex];

        switch (limbToCut)
        {
            case "LeftHand":
                cuttingLimbs.CutLeftHand(false);
            break;

            case "RightHand":
                cuttingLimbs.CutRightHand(false);
            break;

            case "RightLeg":
                cuttingLimbs.CutRightLeg(false);
            break;

            case "LeftLeg":
                cuttingLimbs.CutLeftLeg(false);
            break;

            default:
                Debug.LogError("Unknown limb: " + limbToCut);
            break;
        }
    }
}
