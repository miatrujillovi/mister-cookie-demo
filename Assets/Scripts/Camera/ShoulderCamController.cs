using Unity.Cinemachine;
using UnityEngine;

public class ShoulderCamController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freeLookCam;
    [SerializeField] private CinemachineCamera shoulderCam;
    [SerializeField] private Transform shoulderTarget;
    [SerializeField] private Transform orientation;

    private CinemachinePanTilt panTilt;
    private CinemachineInputAxisController inputController;

    private void Start()
    {
        panTilt = shoulderCam.GetComponent<CinemachinePanTilt>();
        inputController = shoulderCam.GetComponent<CinemachineInputAxisController>();

        // Posiciona la camara en el hombro
        shoulderCam.transform.SetParent(shoulderTarget);
        shoulderCam.transform.localPosition = Vector3.zero;
        shoulderCam.transform.localRotation = Quaternion.identity;

        // Empieza desactivada
        shoulderCam.Priority = 0;
        freeLookCam.Priority = 10;

        // Empieza sin input
        if (inputController != null)
            inputController.enabled = false;
    }

    private void Update()
    {
        // Si la shoulderCam esta activa, sigue la posicion del ShoulderTarget
        if (shoulderCam.Priority > 0)
        {
            shoulderCam.transform.position = shoulderTarget.position;
        }
    }

    public void SetShoulderCam(bool active)
    {
        if (active)
        {
            shoulderCam.transform.SetParent(null);
            shoulderCam.transform.position = shoulderTarget.position;

            // Usa la rotacion del orientation que apunta al frente del player
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