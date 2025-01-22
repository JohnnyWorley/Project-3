using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bossBarrier;
    private bool bossDead = false;
    private MinotaurBoss MinotaurBoss;
    public bool instantiated = false, phase2Enabled = false;
    private float currentTime;
    private float bCurrentTime;
    public GameObject blood;
    public GameObject player;
    [Header("Drops")]
    public GameObject blueGem;
    public GameObject greenGem;
    public GameObject heart;
    [Header("Health")]
    public float maxHealth;
    public float health;
    public Animator animator;
    Color objectColor;


    // Start is called before the first frame update
    void Start()
    {
        MinotaurBoss = FindObjectOfType<MinotaurBoss>();
        health = maxHealth;
    }

    private void Update()
    {
        if (instantiated)
        {
            gameObject.GetComponent<SpiderAI>().aggro = true;
        }
    }
    public void takeDamage(float damage)
    {
       
        health -= damage;
        MinotaurBoss.HealthBarUpdate();
        StartCoroutine(damageFlash());
        if (health <= 0 )
        {
            if (gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(spiderDeath());
            }
           else if (gameObject.CompareTag("Boss")) 
            {
                bossDead = true;
                StartCoroutine(bossDeath());
            }
        }

        if (health <= 25 && gameObject.CompareTag("Boss") && !phase2Enabled)
        {
            phase2Enabled = true;
            MinotaurBoss.phase2();
        }

    }
    
    private IEnumerator spiderDeath()
    {
        gameObject.tag = "Untagged";
        gameObject.GetComponent<Rigidbody2D>().mass = 999999999999;
        gameObject.GetComponent<SpiderAI>().enabled = false;
        animator.SetFloat("Moving", 0);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if (!instantiated) 
        {
            Instantiate(blood, gameObject.transform.position, gameObject.transform.rotation);
        }
        animator.SetTrigger("Death");
        this.GetComponent<Renderer>().sortingOrder = 1;
        int gemPicker = Random.Range(1, 7);
        yield return new WaitForSeconds(2f);
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();  
        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            sr.color = Color.Lerp(sr.color, new Color(255,255,255, 0), (currentTime / 300)); 
            yield return null;
        }

        FindObjectOfType<MinotaurBoss>().enemiesAlive--;
        Destroy(gameObject);

        if (instantiated)
        {
            Instantiate(heart, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            switch (gemPicker)
            {
                case 5:
                    Instantiate(greenGem, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                case 6:
                    Instantiate(heart, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                default:
                    Instantiate(blueGem, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
            }
        }

    }



    private IEnumerator bossDeath()
    {
        MinotaurBoss.GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(bossBarrier);
        animator.SetTrigger("Death");
        if (!bossDead)
        {            
            StopAllCoroutines();
            bossDead = true;
            
        }
        MinotaurBoss.begun = false;
        gameObject.GetComponent<MinotaurBoss>().enabled = false;
        while (true)
        {
            gameObject.transform.position += new Vector3(0,-0.05f,0);
            yield return new WaitForSeconds(0.01f);
        }
       
    }








    private IEnumerator damageFlash()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

    }
}
