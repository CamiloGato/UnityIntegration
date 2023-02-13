using System.Collections;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Transform sight;

    private void Start()
    {
        sight = transform.GetChild(0);
    }
    
    public void Move(GameObject target, float speed)
    {
        sight.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(sight);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
    }

}