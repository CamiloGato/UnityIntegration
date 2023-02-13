using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    [Tooltip("The particles that appear after the player collects a coin.")]
    public GameObject coinParticles;

    PlayerMovement playerMovementScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerMovementScript = other.GetComponent<PlayerMovement>();
            playerMovementScript.soundManager.PlayCoinSound();
            ScoreManager.score += 10;
            ScoreManager.amountCoinsCollected += 1;
            CheckAmountCoinsCollected();
            GameObject particles = Instantiate(coinParticles, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
    
    private void CheckAmountCoinsCollected()
    {
        if (ScoreManager.amountCoinsCollected == 10)
        {
            ScoreManager.amountCoinsCollected = 0;
            playerMovementScript.ChangeHealth(10);
        }
    }
}