using Unity.Cinemachine;
using UnityEngine;

public class ShoulderCamController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freeLookCam;
    [SerializeField] private CinemachineCamera shoulderCam;
    [SerializeField] private Transform shoulderTarget;
    [SerializeField] private Transform orientation;
    [SerializeField] private float blendTime = 0.1f; //controla la velocidad aqui

    private CinemachineBrain cinemachineBrain;
    private CinemachinePanTilt panTilt;
    private CinemachineInputAxisController inputController;

    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        panTilt = shoulderCam.GetComponent<CinemachinePanTilt>();
        inputController = shoulderCam.GetComponent<CinemachineInputAxisController>();

        shoulderCam.transform.SetParent(shoulderTarget);
        shoulderCam.transform.localPosition = Vector3.zero;
        shoulderCam.transform.localRotation = Quaternion.identity;
        shoulderCam.Priority = 0;
        freeLookCam.Priority = 10;

        if (inputController != null)
            inputController.enabled = false;
    }

    private void Update()
    {
        if (shoulderCam.Priority > 0)
            shoulderCam.transform.position = shoulderTarget.position;
    }

    public void SetShoulderCam(bool active)
    {
        // Cambia el blend time solo para esta transicion
        cinemachineBrain.DefaultBlend.Time = blendTime;

        if (active)
        {
            shoulderCam.transform.SetParent(null);
            shoulderCam.transform.position = shoulderTarget.position;

            if (panTilt != null)
            {
                float yaw = orientation.eulerAngles.y;
                float pitch = Camera.main.transform.eulerAngles.x;
                panTilt.PanAxis.Value = yaw;
                panTilt.TiltAxis.Value = pitch > 180f ? pitch - 360f : pitch;
            }

            shoulderCam.Priority = 20;

            if (inputController != null)
                inputController.enabled = true;
        }
        else
        {
            shoulderCam.Priority = 0;
            shoulderCam.transform.SetParent(shoulderTarget);
            shoulderCam.transform.localPosition = Vector3.zero;
            shoulderCam.transform.localRotation = Quaternion.identity;

            if (inputController != null)
                inputController.enabled = false;
        }
    }
}