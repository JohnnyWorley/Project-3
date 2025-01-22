using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;
    WallHealth wallHealth;
    public float damage = 1f;
    public int critChance = 16;
    private float strength = 300f, delay = 1.5f;
    // Start is called before the first frame update


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;

            GiveDamage(enemy);
            StartCoroutine(Knockback(enemy));
        }
        else if(collision.CompareTag("Boss"))
        {
            GameObject enemy = collision.gameObject;
            GiveDamage(enemy);
        }
        else if (collision.CompareTag("Breakable"))
        {
            wallHealth = collision.GetComponent<WallHealth>();
            wallHealth.health--;
            if (collision.GetComponent<WallHealth>().health <= 0)
            {
                wallHealth = null;
                Destroy(collision.gameObject);
            }
            StartCoroutine(flash());
        }
    }

    private void GiveDamage(GameObject enemy)
    {
        int d = Random.Range(1, critChance);
        if (d == critChance - 1)
        {
            enemy.GetComponent<Enemy>().takeDamage(damage * 2);
        }
        else
        {
            enemy.GetComponent<Enemy>().takeDamage(damage);
        }
    }

    private IEnumerator Knockback(GameObject enemy)
    {

        Vector2 direction = (enemy.transform.position - gameObject.transform.parent.transform.position).normalized;

        rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * strength * Time.deltaTime, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(delay); 
       
        rb.velocity = Vector3.zero;
    }


    public IEnumerator flash()
    {
        wallHealth.GetComponent<SpriteRenderer>().color = Color.grey;
        yield return new WaitForSeconds(0.1f);
        wallHealth.GetComponent<SpriteRenderer>().color = Color.white;
    }

}