using UnityEngine;

public class MinionMove : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    private int currentTarget = 0;

    void Update()
    {
        // if (waypoints.Length == 0) return;

        // Transform target = waypoints[currentTarget];
        // transform.position = Vector3.MoveTowards(transform.position,
        //     target.position, speed * Time.deltaTime);

        // if (Vector3.Distance(transform.position, target.position) < 0.1f)
        // {
        //     currentTarget = (currentTarget + 1) % waypoints.Length;
        // }
    }
}
