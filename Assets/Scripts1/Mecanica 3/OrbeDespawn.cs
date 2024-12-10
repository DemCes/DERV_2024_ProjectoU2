using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbeDespawn : MonoBehaviour
{
    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.IncrementOrbCounter();
            gameObject.SetActive(false); 
        }

        
        if (collision.gameObject.CompareTag("IsGround"))
        {
            gameObject.SetActive(false);
        }
    }
}