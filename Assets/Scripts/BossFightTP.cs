using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFightTP : MonoBehaviour
{    
    PlayerController playerController;
    public GameObject player; 
    private void Start()
    {
        image.gameObject.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
    }

    public RawImage image;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            image.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
            playerController.actionOnGoing = true;
        }
    }

    public void Accept()
    {
        image.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        playerController.actionOnGoing = false;
        player.transform.position = new Vector3(-271.38f, -110.01f, 0f);
    }

    public void Deny()
    {
        image.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        playerController.actionOnGoing = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
