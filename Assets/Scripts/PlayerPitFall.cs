using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPitFall : MonoBehaviour
{


    Vector3 endSize = new Vector3(0.1f,0.1f,0.1f);
    float shrinkTime = 2f;
    GameObject player;
    public Canvas canvas;
    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            collision.gameObject.GetComponent<PlayerController>().actionOnGoing = true;
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            player = collision.gameObject;
            canvas.enabled = false;
            StartCoroutine(ShrinkPlayer());
        }
    }


    private IEnumerator ShrinkPlayer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < (shrinkTime - 1.25f))
        {
            elapsedTime += Time.deltaTime;
            player.transform.localScale = Vector3.MoveTowards(player.transform.localScale, endSize, shrinkTime * Time.deltaTime);
            yield return null;
        }
        player.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        playerController.PlayerDeath();
       
    }
}
