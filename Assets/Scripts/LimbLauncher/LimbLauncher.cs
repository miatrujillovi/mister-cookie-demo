using UnityEngine;
using UnityEngine.InputSystem;

public class LimbLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 30f;
    [SerializeField] private float chargeSpeed = 10f;

    [Header("Trajectory")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 30;
    [SerializeField] private float timeBetweenPoints = 0.1f;

    [Header("References")]
    [SerializeField] private GameObject forceCanvas;

    [Header("UI")]
    [SerializeField] private ForceUI forceUI;

    [Header("Camera")]
    [SerializeField] private ShoulderCamController shoulderCamController;

    private GameObject selectedLimb;
    private Rigidbody limbRb;
    private float currentForce;
    private bool isCharging = false;
    private bool limbSelected = false;

    private void Start()
    {
        trajectoryLine.enabled = false;
        forceCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!limbSelected) return;

        if (InputManager.Instance.LaunchPressed)
        {
            isCharging = true;
            currentForce = Mathf.Clamp(currentForce + chargeSpeed * Time.deltaTime, minForce, maxForce);
            forceUI.UpdateForce(currentForce, minForce, maxForce);
            DrawTrajectory();
        }

        if (InputManager.Instance.LaunchReleased && isCharging)
        {
            LaunchLimb();
            isCharging = false;
            currentForce = 0f;
            trajectoryLine.positionCount = 0;
            forceUI.UpdateForce(0, minForce, maxForce);
            limbSelected = false;
        }
    }

    public void SetSelectedLimb(GameObject limb)
    {
        selectedLimb = limb;
        limbRb = limb.GetComponent<Rigidbody>();
        limbSelected = true;
        currentForce = minForce;
        trajectoryLine.enabled = true;
        forceCanvas.SetActive(true);
        shoulderCamController.SetShoulderCam(true);
    }

    private void DrawTrajectory()
    {
        Vector3 launchDir = GetLaunchDirection();
        Vector3 startPos = Camera.main.transform.position;
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
        selectedLimb.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        Vector3 launchDir = GetLaunchDirection();
        limbRb.AddForce(launchDir * currentForce, ForceMode.Impulse);
        trajectoryLine.enabled = false;
        forceCanvas.SetActive(false);
        shoulderCamController.SetShoulderCam(false);
    }

    private Vector3 GetLaunchDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point; // apunta a un objeto en escena
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 50f; // apunta al aire
        }

        Vector3 dir = Camera.main.transform.forward;
        dir.y += 0.3f;
        return dir.normalized;
    }
}