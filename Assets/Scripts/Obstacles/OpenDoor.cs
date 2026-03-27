using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private float openSpeed = 2f;
    private Transform doorPivot;

    private void Start()
    {
        doorPivot = GetComponent<Transform>();
    }

    public void OpeningDoor()
    {
        doorPivot.DOLocalRotate(new Vector3(0f, 90f, 0f), openSpeed);
    }
}
