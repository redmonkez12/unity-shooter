using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Turret Settings")]
    public float rotationSpeed = 150f;
    public float detectionRange = 10f;
    public LayerMask enemyLayerMask = 8;

    [Header("Debug")]
    public bool showDetectionRange = true;

    private Transform currentTarget;

    void Update()
    {
        FindClosestEnemy();
        RotateTowardsTarget();
    }

    void FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayerMask);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = collider.transform;
            }
        }

        currentTarget = closestTarget;
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = (currentTarget.position - transform.position).normalized;

        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }

    // void OnDrawGizmos()
    // {
    //     if (showDetectionRange)
    //     {
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawWireSphere(transform.position, detectionRange);
    //     }

    //     // ZOBRAZ SMĚROVÉ OSPY:
    //     Gizmos.color = Color.red;   // X-axis (RIGHT)
    //     Gizmos.DrawRay(transform.position, transform.right * 2f);

    //     Gizmos.color = Color.green; // Y-axis (UP) 
    //     Gizmos.DrawRay(transform.position, transform.up * 2f);

    //     Gizmos.color = Color.blue;  // Z-axis (FORWARD) - tam kam míří
    //     Gizmos.DrawRay(transform.position, transform.forward * 3f);

    //     // Pokud má cíl, zobraz směr
    //     if (currentTarget != null)
    //     {
    //         Gizmos.color = Color.magenta;
    //         Vector3 targetDirection = (currentTarget.position - transform.position).normalized;
    //         targetDirection.y = 0;
    //         Gizmos.DrawRay(transform.position, targetDirection * 4f);
    //     }
    // }
}