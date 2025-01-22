using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMerchant : MonoBehaviour
{
    private EnterMerchant script;
    public Vector3 prevPos;
    private PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.grappleEnabled = true;
            collision.transform.position = prevPos - new Vector3 (0,1,0);
        }
    }
}