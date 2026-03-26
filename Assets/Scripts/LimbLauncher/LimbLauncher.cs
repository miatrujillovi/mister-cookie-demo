using Unity.Cinemachine;
using UnityEngine;

public class LimbLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 30f;

    [Header("Trajectory")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 30;
    [SerializeField] private float timeBetweenPoints = 0.1f;

    [Header("References")]
    [SerializeField] private GameObject forceCanvas;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("UI")]
    [SerializeField] private ForceUI forceUI;

    [Header("Camera")]
    [SerializeField] private ShoulderCamController shoulderCamController;
    [SerializeField] private CinemachineCamera shoulderCam;

    [Header("Hold Settings")]
    [SerializeField] private Transform holdPoint;
    private bool isHolding = false;

    private GameObject selectedLimb;
    private Rigidbody limbRb;
    private float currentForce;
    private bool isCharging = false;
    private bool limbSelected = false;

    private void Start()
    {
        trajectoryLine.useWorldSpace = true;
        trajectoryLine.enabled = false;
        forceCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!limbSelected) return;

        if (isHolding && selectedLimb != null)
        {
            selectedLimb.transform.position = holdPoint.position;
            selectedLimb.transform.rotation = holdPoint.rotation;
        }

        // Ajusta la fuerza con el scroll
        float scroll = InputManager.Instance.ScrollInput;
        if (scroll != 0f)
        {
            currentForce = Mathf.Clamp(currentForce + scroll * 1.2f, minForce, maxForce);
            forceUI.UpdateForce(currentForce, minForce, maxForce);
            DrawTrajectory();
        }

        if (InputManager.Instance.LaunchPressed)
        {
            isCharging = true;
            DrawTrajectory();
        }

        if (InputManager.Instance.LaunchReleased && isCharging)
        {
            LaunchLimb();
            isCharging = false;
            currentForce = minForce; // resetea a fuerza minima
            trajectoryLine.positionCount = 0;
            forceUI.UpdateForce(0, minForce, maxForce);
            limbSelected = false;
        }
    }

    public void SetSelectedLimb(GameObject limb)
    {
        selectedLimb = limb;
        limbRb = limb.GetComponent<Rigidbody>();
        limbRb.isKinematic = true;
        limbSelected = true;
        isHolding = true;
        currentForce = minForce;
        trajectoryLine.enabled = true;
        forceCanvas.SetActive(true);
        playerMovement.enabled = false;
        shoulderCamController.SetShoulderCam(true);
    }

    private void DrawTrajectory()
    {
        Vector3 launchDir = GetLaunchDirection();
        Vector3 startPos = selectedLimb.transform.position;
        trajectoryLine.positionCount = trajectoryPoints;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeBetweenPoints;
            Vector3 point = startPos
                + launchDir * currentForce * t
                + 0.5f * Physics.gravity * t * t;
            trajectoryLine.SetPosition(i, point);
        }
    }

    private void LaunchLimb()
    {
        if (selectedLimb == null || limbRb == null) return;
        limbRb.isKinematic = false;
        limbRb.linearVelocity = Vector3.zero;
        Vector3 launchDir = GetLaunchDirection();
        limbRb.AddForce(launchDir * currentForce, ForceMode.Impulse);
        trajectoryLine.enabled = false;
        forceCanvas.SetActive(false);
        shoulderCamController.SetShoulderCam(false);
        playerMovement.enabled = true;

        InputManager.Instance.EnableSelection();
    }

    private Vector3 GetLaunchDirection()
    {
        Transform camTransform = shoulderCam.transform;
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point; // apunta a un objeto en escena
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 50f; // apunta al aire
        }

        Vector3 dir = (targetPoint - selectedLimb.transform.position).normalized;
        return dir;
    }
}