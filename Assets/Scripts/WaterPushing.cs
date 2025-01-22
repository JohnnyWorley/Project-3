using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPushing : MonoBehaviour
{


    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    int inWater = 0;
    Rigidbody2D rb;
   // GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inWater++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")|| collision.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Player"))
            {
                playerController.grappleEnabled = false;
                playerController.speed = 3;
            }
            else if (collision.CompareTag("Enemy"))
              
            {
                collision.GetComponent<SpiderAI>().speed = 2.2f;
               
            }
            rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * (5000 / inWater) * Time.deltaTime, ForceMode2D.Force);
            rb.velocity = Vector2.zero;
            
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inWater--;
        if (collision.CompareTag("Player"))
        {
            playerController.grappleEnabled = true;
            playerController.speed = 5;
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<SpiderAI>().speed = 2;
            Rigidbody2D spiderRB = collision.GetComponent<Rigidbody2D>();
            spiderRB.velocity = Vector2.zero;
        }
    }
}   
