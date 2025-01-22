using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMerchant : MonoBehaviour
{
    [SerializeField]
    private ExitMerchant exitMerchant;
    public GameObject entrance;
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.grappleEnabled = false;
            entrance = collision.gameObject;
            exitMerchant.prevPos = collision.transform.position;
            collision.transform.position = new Vector3(-60.5f, 7.5f, 0f); ;
        }
    }
}
