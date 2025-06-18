using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public float turnSpeed;

    [Header("Idle data")]
    public float idleTime;

    [Header("Move data")]
    public float moveSpeed;

    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex;

    public Animator anim;

    public NavMeshAgent agent { get; private set; }

    public MinionStateMachine stateMachine { get; set; }

    protected virtual void Awake()
    {
        stateMachine = new MinionStateMachine();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
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

    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currenEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currenEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        return Quaternion.Euler(currenEulerAngles.x, yRotation, currenEulerAngles.z);
    }
}
