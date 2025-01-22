using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PitFall : MonoBehaviour
{

    float shrinkTime = 1f;
    Vector3 endSize = Vector3.zero;

    bool entered = false;
    private List<Transform> enemies = new List<Transform>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SpiderAI>().enabled = false;
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Enemy>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Transform enemyTransform = collision.transform;
            enemies.Add(enemyTransform);
            FindObjectOfType<MinotaurBoss>().enemiesAlive--;

            if (!entered)
            {
                entered = true;
                StartCoroutine(ShrinkEnemy());
            }
        }
    }
   
    
    private IEnumerator ShrinkEnemy()
    {
        while (enemies.Count > 0)
        {
            for(int i = enemies.Count - 1; i >= 0; i--)
            {
                Transform enemyTransform = enemies[i];
                if (enemyTransform.localScale != endSize)
                {
                    enemyTransform.localScale = Vector3.MoveTowards(enemyTransform.localScale, endSize, shrinkTime * Time.deltaTime);
                }
                else
                {
                    enemies.Remove(enemyTransform);
                    Destroy(enemyTransform.gameObject);
                }
            }
            yield return null;
        }
        entered = false;
    }


  

}
