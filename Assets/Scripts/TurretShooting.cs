using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 1f;
    public float bulletLifetime = 2f; // Stejně jako u hráče

    [Header("Gun Points")]
    public Transform[] gunPoints;

    [Header("Accuracy Settings")]
    public float accuracy = 0.8f; // 0-1, kde 1 = perfektní přesnost
    public float maxSpread = 2f; // Maximální rozptyl v metrech

    private float nextFireTime = 0f;
    private int currentGunIndex = 0;
    private TurretController turretController;

    // Konstanta z PlayerWeaponController
    private const float REFERENCE_BULLET_SPEED = 20;

    private void Start()
    {
        turretController = GetComponent<TurretController>();
    }

    private void Update()
    {
        if (turretController.HasTarget() && CanShoot())
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        return Time.time >= nextFireTime;
    }

    private void Shoot()
    {
        if (gunPoints.Length == 0) return;

        AlignGunPoints();

        Transform currentGunPoint = gunPoints[currentGunIndex];
        Vector3 spawnPosition = currentGunPoint.position + currentGunPoint.forward * 0.3f;

        GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(currentGunPoint.forward));

        // IGNORUJ KOLIZE S TURRETEM
        Collider bulletCollider = newBullet.GetComponentInChildren<Collider>();
        Collider turretCollider = GetComponentInChildren<Collider>(); // Nebo GetComponentInParent<Collider>()

        if (bulletCollider != null && turretCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, turretCollider);
        }

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection(currentGunPoint) * bulletSpeed;

        Destroy(newBullet, bulletLifetime);

        nextFireTime = Time.time + (1f / fireRate);
        currentGunIndex = (currentGunIndex + 1) % gunPoints.Length;
    }

    private void AlignGunPoints()
    {
        if (gunPoints.Length == 0) return;

        Transform target = turretController.GetCurrentTarget();
        if (target == null) return;

        // Pro každý gun point nastav správnou orientaci
        foreach (Transform gunPoint in gunPoints)
        {
            Vector3 direction = (target.position - gunPoint.position).normalized;
            gunPoint.rotation = Quaternion.LookRotation(direction);
        }
    }

    private Vector3 PredictTargetPositionAdvanced(Transform target, Vector3 shooterPosition)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null) return target.position;

        Vector3 targetPos = target.position;
        Vector3 targetVel = targetRb.linearVelocity;

        // Iterativní výpočet (přesnější)
        for (int i = 0; i < 3; i++) // 3 iterace pro přesnost
        {
            float distance = Vector3.Distance(shooterPosition, targetPos);
            float time = distance / bulletSpeed;
            targetPos = target.position + (targetVel * time);
        }

        return targetPos;
    }

    private Vector3 BulletDirection(Transform gunPoint)
    {
        Transform target = turretController.GetCurrentTarget();
        if (target == null) return gunPoint.forward;

        // Predikovaná pozice
        Vector3 predictedPosition = PredictTargetPositionAdvanced(target, gunPoint.position);

        // PŘIDEJ NEPŘESNOST
        Vector3 spread = CalculateSpread();
        Vector3 finalTarget = predictedPosition + spread;

        Vector3 direction = (finalTarget - gunPoint.position).normalized;
        return direction;
    }

    private Vector3 CalculateSpread()
    {
        // Čím nižší accuracy, tím větší rozptyl
        float spreadAmount = maxSpread * (1f - accuracy);

        Vector3 spread = new Vector3(
            Random.Range(-spreadAmount, spreadAmount),
            Random.Range(-spreadAmount * 0.5f, spreadAmount * 0.5f), // Méně vertikální rozptyl
            Random.Range(-spreadAmount, spreadAmount)
        );

        return spread;
    }

}