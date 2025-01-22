using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    //Numbers
    public float speed = 2;
    float prevRotation;
    float posTimer = 2.1f;
    float detectDistance = 5f;
    float attackDistance = 3f;
    float playerDistance;
    float deAggroTimer = 4f;

    //Bools
    private bool transition = false;
    public bool aggro = false;
    private bool attacking = false;
    private bool attackFinished = true;

    //Classes
    private Rigidbody2D rb;
    public GameObject player;
    public Animator animator;
    private Vector2 randomPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        Rotation();
        StartCoroutine(posGenerator());
        animator.SetFloat("Moving", 1); 

    }




    void Update()
    {
       

        if (!aggro)
        {
            Rotation();
            playerDistance = (transform.position - player.transform.position).magnitude;
            transform.position = Vector2.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, randomPos) < 0.1f && !transition || posTimer <= 0)
            {
                StartCoroutine(posGenerator());
            }
        }
        else if (aggro) 
        {

            if (!attacking && attackFinished)
            {
                aggroRotation();

            }

            else if (attacking && attackFinished)
            {
                StartCoroutine(attackLunge());
            }
        }


        if (playerDistance < detectDistance)
        {
            aggro = true;
        }
    }


    private void givePlayerDamage()
    {
        player.GetComponent<PlayerController>().takeDamage(1);
    }

    private void aggroRotation()
    {
        playerDistance = (transform.position - player.transform.position).magnitude;
        transform.up = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 3.5f * Time.deltaTime);
        if (playerDistance < attackDistance) 
        {
            attacking = true;
        }
    }

    private IEnumerator attackLunge()
    {
        attackFinished = false;
        Vector3 attackPos = player.transform.position;
        transform.up = attackPos - transform.position;
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(transform.up * 700f * Time.fixedDeltaTime, ForceMode2D.Impulse);

        while (Mathf.Approximately(rb.velocity.magnitude, 0f))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        attackFinished = true;
        attacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.tag != "Enemy")
        {
            deAggroTimer = 4f;
            if (collision.transform.CompareTag("Player") && gameObject.transform.CompareTag("Enemy"))
            {
                givePlayerDamage();
                StartCoroutine(knockback());
            }
            else if (!aggro)
            {
                Reset();
            }
        }

        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag != "Enemy")
        {
            if (!aggro)
            {
                posTimer -= Time.deltaTime;
            }

            else if (aggro)
            {
                deAggroTimer -= Time.deltaTime;
                if (deAggroTimer <= 0f)
                {
                    aggro = false;
                    deAggroTimer = 4f;
                }
            }
        }
    }

    private IEnumerator posGenerator()
    {
        posTimer = 2.1f;
        animator.SetFloat("Moving", 0);
        transition = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.00f, 2f));
        speed = 2;
        randomPos = transform.position - new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), 1);
        transition = false;
        animator.SetFloat("Moving", 1);
    }
    void Reset()
    {
        speed = 0;
        StopAllCoroutines();
        StartCoroutine(posGenerator());
    }

   
    private IEnumerator knockback()
    {
       Vector2 direction = (player.transform.position - gameObject.transform.position).normalized;

       Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            playerRB.AddForce(direction * 600f * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        player.GetComponent<PlayerController>().actionOnGoing = true;

        yield return new WaitForSeconds(0.5f);
        playerRB.velocity = Vector2.zero;
        player.GetComponent<PlayerController>().actionOnGoing = false;
    }
  


    void Rotation()
    {
       
            Vector2 direction = (randomPos - (Vector2)transform.position).normalized; // gets direction and makes magntidude 1 just cos easier
            float angle = Mathf.Atan2(direction.y, direction.x); // radians
            float angleDegrees = angle * Mathf.Rad2Deg; // converts radians to degrees
            prevRotation = transform.rotation.eulerAngles.z;
            if (!transition)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, (angleDegrees) - 90));

            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, prevRotation)); // just keeps it in the same direction
            }
    }




}


