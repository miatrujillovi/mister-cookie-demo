using Unity.Cinemachine;
using UnityEngine;

public class ShoulderCamController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freeLookCam;
    [SerializeField] private Transform shoulderTarget; // arrastra tu punto aquí

    private CinemachineOrbitalFollow orbitalFollow;
    private Vector3 originalOffset;
    private float originalDistance;
    private float shoulderDistance = 2.5f;

    private void Start()
    {
        orbitalFollow = freeLookCam.GetComponent<CinemachineOrbitalFollow>();
        originalOffset = orbitalFollow.TargetOffset;
        originalDistance = orbitalFollow.Orbits.Center.Radius;
    }

    public void SetShoulderCam(bool active)
    {
        if (active)
        {
            // Usa la posicion local del target como offset
            orbitalFollow.TargetOffset = shoulderTarget.localPosition;
            orbitalFollow.Orbits.Center.Radius = shoulderDistance;
        }
        else
        {
            orbitalFollow.TargetOffset = originalOffset;
            orbitalFollow.Orbits.Center.Radius = originalDistance;
        }
    }
}