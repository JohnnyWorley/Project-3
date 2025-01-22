using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopBuy : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private Canvas sB;
    private Collectable collectable;
    private PlayerController playerController;
    bool inTrigger = false;
    private void Start()
    {
        sB = gameObject.GetComponentInChildren<Canvas>();
        collectable = GetComponent<Collectable>();
        playerController = FindObjectOfType<PlayerController>();
        
    }


    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && collectable.price <= playerController.gems)
            {
                string itemType = gameObject.GetComponent<Collectable>().itemType;
                playerController.gems = playerController.gems - collectable.price;
                GameManager.instance.itemBuy(itemType);
                if (gameObject.name != "heartBuy")
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "E to Buy";
            interactText.transform.parent.transform.gameObject.SetActive(true);
            inTrigger = true;
           sB.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.transform.parent.transform.gameObject.SetActive(false);
            inTrigger = false;
            sB.enabled = false;
        }
    }



}
