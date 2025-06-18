using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [Header("Idle data")]
    public float idleTime;

    [Header("Move data")]
    public float moveSpeed;

    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex;

    public NavMeshAgent agent { get; private set; }

    public MinionStateMachine stateMachine { get; set; }

    protected virtual void Awake()
    {
        stateMachine = new MinionStateMachine();

        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    protected virtual void Update()
    {

    }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    private void InitializePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }

}
