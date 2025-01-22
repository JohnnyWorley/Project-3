using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class MinotaurBoss : MonoBehaviour
{
    bool stunned = false;

    public bool begun = false;
    private Enemy enemyManager;
    private PlayerController playerController;

    [Header("UI Elements")]
    public Image healthBar;

   public GameObject spider;
   public GameObject portal;
   public GameObject player;
   public GameObject axe;
   public GameObject FireBall;


   public Animator animator;

   public int enemiesAlive = 0;
   public float lowerTime = 4.5f, upperTime = 6.5f;
    private Rigidbody2D playerRB;
    [SerializeField]
    private int attackCycle;
    private int attackCycleAmount;
    // Start is called before the first frame update
    void Awake()
    {
        
        healthBar.fillAmount = 1;
        playerRB = player.GetComponent<Rigidbody2D>();
        attackCycleAmount = Random.Range(4, 7); // 4 - 6
        playerController = FindObjectOfType<PlayerController>();
        enemyManager = GetComponent<Enemy>();
        enemiesAlive = 0;
        StartCoroutine(bossFight());
    }

    // Update is called once per frame
   


    private IEnumerator bossFight()
    {
       while (!begun)
        {
            yield return null;
        }
        int randomAttack = Random.Range(1, 6); //1 - 5
        if (attackCycle <= attackCycleAmount)
        {
           
            switch (randomAttack)
            {
                case 1:
                    if (enemiesAlive <= 0)
                    {
                        StartCoroutine(SpiderSummon());
                    }
                    break;

                case 2:
                    StartCoroutine(FireBallThrow());
                    break;

                case 3:
                    StartCoroutine(FireBallThrow());
                    break;

                case 4:
                    StartCoroutine(AxeThrow());
                    break;

                case 5:
                    StartCoroutine(AxeThrow());
                    break;
            }
            yield return new WaitForSeconds(Random.Range(lowerTime, upperTime));
            attackCycle++;
            StartCoroutine(bossFight());
        }
        else
        {
            stunned = true;
            animator.SetTrigger("Stunned");
            yield return new WaitForSeconds(7);
            attackCycleAmount = Random.Range(4, 7); // 4 - 6
            attackCycle = 0;
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(1);
            stunned = false;
            StartCoroutine(bossFight());
        }
    }

    public void phase2()
    {
        lowerTime = (lowerTime / 2) + 1f;
        upperTime = (upperTime / 2) + 1f;
    }

    private IEnumerator AxeThrow()
    {
        animator.SetTrigger("AxeAttack");
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Idle");


        GameObject axeInstantie1 = Instantiate(axe, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        GameObject axeInstantie2 = Instantiate(axe, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        GameObject axeInstantie3 = Instantiate(axe, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Vector3 prevPos = player.transform.position;
        axeInstantie1.transform.up = prevPos - transform.position;

        axeInstantie2.transform.up = prevPos - transform.position;
        axeInstantie2.transform.Rotate(0, 0, 15);

        axeInstantie3.transform.up = prevPos - transform.position;
        axeInstantie3.transform.Rotate(0, 0, -15);

        yield return new WaitForSeconds(5);

        Destroy(axeInstantie1);
        Destroy(axeInstantie2);
        Destroy(axeInstantie3);

    }

    private IEnumerator FireBallThrow()
    {
        animator.SetTrigger("FireAttack");
        for (int i = 0; i < 10; i++)
        {
            Vector3 prevPos = player.transform.position;
            GameObject fireball = Instantiate(FireBall, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            fireball.transform.up = prevPos - transform.position;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Idle");
    }


    private IEnumerator SpiderSummon()
    {
        animator.SetTrigger("FireAttack");
        GameObject portal1 = Instantiate(portal, gameObject.transform.position + new Vector3(-10, 3  ,0), Quaternion.Euler(0, 0, 0));
        GameObject portal2 = Instantiate(portal, gameObject.transform.position + new Vector3(-10, -3 ,0), Quaternion.Euler(0, 0, 0));

        yield return new WaitForSeconds(2);

        Destroy(portal1); Destroy(portal2);

        GameObject spider1 = Instantiate(spider, gameObject.transform.position + new Vector3(-10,3  ,0), Quaternion.Euler(0, 0, 0));
        GameObject spider2 = Instantiate(spider, gameObject.transform.position + new Vector3(-10,-3 ,0), Quaternion.Euler(0, 0, 0));
        spider1.GetComponent<SpiderAI>().aggro = true;
        spider1.GetComponent <SpiderAI>().player = player;
        spider1.GetComponent<Enemy>().instantiated = true;

        spider2.GetComponent<SpiderAI>().aggro = true;
        spider2.GetComponent<SpiderAI>().player = player;
        spider2.GetComponent<Enemy>().instantiated = true;
        animator.SetTrigger("Idle");
        enemiesAlive = 2;
        yield return null;
    }

    private void givePlayerDamage()
    {
        player.GetComponent<PlayerController>().takeDamage(1);
    }

    private IEnumerator knockback()
    {
           Vector2 direction = (player.transform.position - gameObject.transform.position).normalized;

            Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();

            if (playerRB != null)
            {
                playerRB.AddForce(direction * 600f * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            player.GetComponent<PlayerController>().actionOnGoing = true;

            yield return new WaitForSeconds(0.5f);
            playerRB.velocity = Vector2.zero;
            player.GetComponent<PlayerController>().actionOnGoing = false;

           

    }

    public void HealthBarUpdate()
    {
        if (enemyManager.health <= 25)
        {
            healthBar.color = Color.magenta;
        }
        healthBar.fillAmount = enemyManager.health / enemyManager.maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && !stunned)
        {

            StartCoroutine(knockback());
            givePlayerDamage();
           
        }
    }


}
