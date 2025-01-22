using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    private PlayerController playerController;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI scoreCounter;
    public TextMeshProUGUI chestAmount;
    private bool inTrigger = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Open");
            int rng = Random.Range(15, 26);
           
                playerController.gems = playerController.gems + rng;
            
           
            playerController.chestsOpened++;
            chestAmount.text = ("Chests Found " + playerController.chestsOpened+"/5");
            scoreCounter.text = ("x" + playerController.gems.ToString());
            interactText.transform.parent.transform.gameObject.SetActive(false);
            Destroy(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.transform.parent.transform.gameObject.SetActive(true);
            interactText.text = "E to Open";
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.transform.parent.transform.gameObject.SetActive(false);
            inTrigger = false;
        }
    }

}
