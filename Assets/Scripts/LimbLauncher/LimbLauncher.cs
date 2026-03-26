using Unity.Cinemachine;
using UnityEngine;
using ProjectileCurveVisualizerSystem;

public class LimbLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 30f;

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

    [Header("Projectile Visualizer")]
    [SerializeField] private ProjectileCurveVisualizer projectileCurveVisualizer;

    private GameObject selectedLimb;
    private Rigidbody limbRb;
    private float currentForce;
    private bool isCharging = false;
    private bool limbSelected = false;
    private Vector3 updatedProjectileStartPosition;
    private RaycastHit hit;

    private void Start()
    {
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
        forceCanvas.SetActive(true);
        playerMovement.enabled = false;
        shoulderCamController.SetShoulderCam(true);
        projectileCurveVisualizer.HideProjectileCurve();
    }

    private void DrawTrajectory()
    {
        Vector3 launchDir = GetLaunchDirection();

        Vector3 launchVelocity = launchDir * currentForce;

        projectileCurveVisualizer.VisualizeProjectileCurve(
            selectedLimb.transform.position, // origen
            1.0f,                            // masa (puedes dejarlo así)
            launchVelocity,                  // velocidad inicial
            0.1f,                            // resolución
            0.05f,                           // step
            true,                            // usar raycast
            out updatedProjectileStartPosition,
            out hit
        );
    }

    private void LaunchLimb()
    {
        if (selectedLimb == null || limbRb == null) return;
        limbRb.isKinematic = false;
        limbRb.linearVelocity = Vector3.zero;
        Vector3 launchDir = GetLaunchDirection();
        Vector3 launchVelocity = launchDir * currentForce;
        selectedLimb.transform.position = updatedProjectileStartPosition;
        limbRb.AddForce(launchVelocity, ForceMode.Impulse);
        projectileCurveVisualizer.HideProjectileCurve();
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

        Vector3 dir = (targetPoint - holdPoint.position).normalized;
        return dir;
    }
}