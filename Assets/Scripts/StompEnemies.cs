using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemies : MonoBehaviour
{
    [Tooltip("The particles that will fly out of enemies after they are defeated.")]
    public GameObject enemyParticles, trapParticles;

    [Tooltip("The transform that detects enemies beneath the player to stomp.")]
    public Transform stompCheck;

    [Tooltip("The layer that the player can stomp on.")]
    public LayerMask enemyHeads;

    private PlayerMovement playerMovementScript;
    private GetHit getHitScript;
    private Rigidbody rb;
    private bool canKill;
    PacingEnemy pacingEnemyScript;
    Animator enemyAnimator;
    GameObject enemyObject;
    private static readonly int Stomped = Animator.StringToHash("Stomped");

    void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        getHitScript = GetComponent<GetHit>();
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {

        // creates a linecast to detect when the player can stomp an enemy's head
        bool stomp = Physics.Linecast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), stompCheck.position, enemyHeads);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), stompCheck.position, Color.red);

        canKill = stomp;

    }
    void OnCollisionEnter(Collision other)
    {
        if (canKill == true)
        {
            if (getHitScript.hurt == false)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    enemyObject = other.gameObject;
                    enemyAnimator = enemyObject.GetComponent<Animator>();
                    enemyAnimator.SetTrigger(Stomped);
                    Kill();
                    StartCoroutine(nameof(DestroyEnemy));
                }

                if (other.gameObject.CompareTag("Trap"))
                {
                    pacingEnemyScript = other.gameObject.GetComponent<PacingEnemy>();
                    pacingEnemyScript.enemyStats.speed = 0;
                    enemyObject = other.gameObject;
                    Kill();
                    StartCoroutine(nameof(DestroyTrap));
                }
            }
        }
    }
    void Kill()
    {
        enemyObject.tag = "Untagged";
        enemyObject.transform.localScale = new Vector3(1.5f, 0.5f, 1.5f);
        rb.AddForce(Vector3.up * 1000);
        playerMovementScript.soundManager.PlayStompSound();
        playerMovementScript.soundManager.PlayEnemyDeathSound();
        ScoreManager.score += 100;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject particles = Instantiate(enemyParticles, enemyObject.transform.position, new Quaternion());
        playerMovementScript.soundManager.PlayEnemyPoofSound();
        Destroy(enemyObject);
    }

    IEnumerator DestroyTrap()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject particles = Instantiate(trapParticles, enemyObject.transform.position, new Quaternion());
        playerMovementScript.soundManager.PlayEnemyPoofSound();
        Destroy(enemyObject);
    }
}