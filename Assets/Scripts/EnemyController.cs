using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Contains tunable parameters to tweak the enemy's movement and behavior.
/// </summary>
[System.Serializable]
public struct Stats
{
    [Header("Enemy Settings")]
    [Tooltip("How fast the enemy walks (only when idle is true).")]
    public float walkSpeed;

    [Tooltip("How fast the enemy turns in circles as they're walking (only when idle is true).")]
    public float rotateSpeed;

    [Tooltip("How fast the enemy runs after the player (only when idle is false).")]
    public float chaseSpeed;

    [Tooltip("How close the enemy needs to be to explode")]
    public float explodeDist;

}

public class EnemyController : MonoBehaviour
{
    
    [Tooltip("The enemy's stats.")]
    public Stats enemyStats;

    [Tooltip("Blue explosion particles")]
    public GameObject enemyExplosionParticles;

    // Whether the enemy is idle or not. Once the player is within distance, idle will turn false and the enemy will chase the player.
    private bool idle;
    private bool slipping;
    private float facing;
    [HideInInspector]public bool canPatrol = true;
    
    private GameObject player;
    private EnemyPatrol patrolScript;
    private EnemyChase chaseScript;

    private void Start()
    {
        patrolScript = GetComponent<EnemyPatrol>();
        chaseScript = GetComponent<EnemyChase>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void Update()
    {
        // changes the enemy's behavior: pacing in circles or chasing the player
        if (idle && canPatrol)
        {
            patrolScript.Move(enemyStats.walkSpeed, enemyStats.rotateSpeed);
        }
        else
        {
            chaseScript.Move(player, enemyStats.chaseSpeed);
            CheckExplode();
        }
        CheckSlipping();
    }
    
    #region Collision Detection
    
    private void OnCollisionEnter(Collision other)
    {
        slipping = other.gameObject.layer == 9;
    }

    private void OnTriggerEnter(Collider other)
    {
        //start chasing if the player gets close enough
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            idle = false;
        }
    }

   private void OnTriggerExit(Collider other)
    {
        //stop chasing if the player gets far enough away
        if (other.gameObject.CompareTag("Player"))
        {
            idle = true;      
        }
    }
   
   #endregion
   
   private bool CanExplode => Vector3.Distance(transform.position, player.transform.position) < enemyStats.explodeDist
                              && !idle;

   private void CheckExplode()
   {
       //Explode if we get within the enemyStats.explodeDist
       if (CanExplode)
       {
           StartCoroutine(nameof(Explode));
           idle = true;
       }
   }

   private void CheckSlipping()
   {
       // stops enemy from following player up the inaccessible slopes
       if (slipping)
       {
           transform.Translate(Vector3.back * (20 * Time.deltaTime), Space.World);
       }
   }

   private IEnumerator Explode()
    {
        GameObject particles = Instantiate(enemyExplosionParticles, transform.position, new Quaternion());
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.parent.gameObject);
    }
}