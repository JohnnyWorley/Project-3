using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    PlayerController playerController;
    Enemy enemy;
    private Rigidbody2D rb;
    private float speed = 10;
    void Start()
    {
        if (gameObject.transform.name == "Axe(Clone)")
        {
            speed = 11 * 1.5f;
        }
        else
        {
            speed = 13.5f * 1.5f;
        }
        playerController = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<Enemy>();
       rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            if (gameObject.name == "Axe(Clone)")
            {
                playerController.takeDamage(1);
                playerController.takeDamage(1);
                Destroy(gameObject);
            }
            else
            {
                playerController.takeDamage(1);
                Destroy(gameObject);
            }

        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            enemy.takeDamage(2);
            Destroy(gameObject);
        }

        else if (collision.transform.CompareTag("GrappleableWall"))
        {
            Destroy(gameObject);
        }
    }
}



