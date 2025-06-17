using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private Vector2 aimInput;
    private RaycastHit lastKnownMouseHit;

    [Header("Aim visuals")]
    [SerializeField] private LineRenderer aimLaser;

    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisely;
    [SerializeField] private bool isLockingToTarget;

    [Header("Camera control")]
    [Range(.5f, 1)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1, 3f)]
    [SerializeField] private float maxCameraDistance = 4f;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensitivity = 5f;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private Transform cameraTarget;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    public bool CanAimPrecisely()
    {
        return isAimingPrecisely;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisely = !isAimingPrecisely;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        UpdateAimLaser();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimLaser()
    {
        Transform gunPoint = player.weaponController.GunPoint();
        Vector3 laserDirection = player.weaponController.BulletDirection();

        float laserTipLength = 0.5f;
        float gunDistance = 4f;


        Vector3 endpoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance, aimLayerMask))
        {
            endpoint = hit.point;
            laserTipLength = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endpoint);
        aimLaser.SetPosition(2, endpoint + laserDirection * laserTipLength);
    }

    public Transform Target()
    {
        Transform target = null;

        if (GetMousePosition().transform.GetComponent<Target>() != null)
        {
            target = GetMousePosition().transform;
        }

        return target;
    }


    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), Time.deltaTime * cameraSensitivity);
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }

            return;
        }

        aim.position = GetMousePosition().point;

        if (isAimingPrecisely == false)
        {
            aim.position = new Vector3(aim.position.x, aim.position.y + 1f, aim.position.z);
        }
    }

    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance = player.movement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMousePosition().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);
        desiredCameraPosition = transform.position + aimDirection * clampedDistance;

        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMousePosition()
    {
        // Použij aktuální pozici myši na obrazovce
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        // Fallback - pokud paprsek nic netrefí, vrať pozici před hráčem
        return lastKnownMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}