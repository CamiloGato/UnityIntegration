using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Tooltip("The transform to which the enemy will pace back and forth to.")]
    public Transform[] patrolPoints;
    
    private int currentPatrolPoint = 0;


    /// <summary>
    /// Patrol Logic movement
    /// </summary>
    /// <param name="speed">Float Speed</param>
    /// <param name="rotateSpeed">Float rotateSpeed</param>
    public void Move(float speed, float rotateSpeed)
    {
        Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, moveToPoint, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint > patrolPoints.Length - 1)
            {
                currentPatrolPoint = 0;
            }
        }
    }

}