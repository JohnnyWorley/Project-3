using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    //Game Classes
    [Header("UI Elements")]
    public RawImage deathBackground;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Canvas canvas;

    [Header("Game Classes")]
    public GameObject pit;
    public GameObject waterPush;
    public GameObject playerFallPit;
    public Sprite[] heartTextures;
    public Image[] hearts;
    public LineRenderer rope;
    public GameObject player;
    public Animator animator;
    public Rigidbody2D rb;
    private string currentScene;
    [Header("Hit Managers")]
    public GameObject hitDetector;
    public BoxCollider2D hitCollider;

    //Bools
    [Header("Bools")]
    public bool playerAlive = true;
    public bool grappleEnabled = true;
    public bool actionOnGoing;

    //Vectors
    private Vector3 origPos, targetPos;
    private Vector3 playerLastDirection;
    Vector2 movement;

    //Floats
    private float grappleShootMoveTime = 0.35f;
    private float grappleOffsetX;
    private float grappleOffsetY;
    private float LastDirectonX;
    private float LastDirectonY;
    private int fullHeart = 0;
    private int halfHeart = 1;
    private int emptyHeart = 2;

    [Header("Statistics")]
    public int chestsOpened = 0;
    public int speed = 5;
    public int gems = 0;
    public int maxHealth = 6;
    private int grappleRange = 10;
   
    public int health;

    [Header("Miscellanous")]
    //Miscellanous
    [SerializeField]
    private AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {

        currentScene = SceneManager.GetActiveScene().name;
        rope.useWorldSpace = true;
        for (int i = 0; i < hearts.Length ; i++)
        {
            hearts[i].sprite = heartTextures[0];
        }
        health = maxHealth;
        hitCollider.offset = new Vector2(0, -1);
        hitCollider.size = new Vector2(0.5f, 0.75f);
        animator.SetFloat("LastDirectionX", 0);
        animator.SetFloat("LastDirectionY", -1);
        playerLastDirection = Vector3.down;
        grappleOffsetY = -0.5f;
        grappleOffsetX = 0f;
        rope.enabled = false;
        rope.SetPosition(1,transform.position);
        rope.SetPosition(0,transform.position);
        grappleEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!actionOnGoing)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                movement.y = 0;
            }
            else
            {
                movement.x = 0;
            }                       
        }
        if (!actionOnGoing)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && grappleEnabled)
        {
            if (actionOnGoing == false)
            {
                StartCoroutine(GrappleHook(playerLastDirection));                
            }
        }

        if (!actionOnGoing)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
       
        if (Input.GetKey(KeyCode.W) && !actionOnGoing)
        {
            hitCollider.offset = new Vector2(0, 1);
            hitCollider.size = new Vector2(0.6f, 1f);
            animator.SetFloat("LastDirectionX", 0);
            animator.SetFloat("LastDirectionY", 1);
            playerLastDirection = Vector3.up;
            grappleOffsetY = .5f;
            grappleOffsetX = 0f;
        }

        if (Input.GetKey(KeyCode.A) && !actionOnGoing)
        {
            hitCollider.offset = new Vector2(-0.75f, 0);
            hitCollider.size = new Vector2(1, 0.6f);
            animator.SetFloat("LastDirectionX", -1);
            animator.SetFloat("LastDirectionY", 0);
            playerLastDirection = Vector3.left;
            grappleOffsetX = -0.5f;
            grappleOffsetY = 0f;

        }

        if (Input.GetKey(KeyCode.S) && !actionOnGoing)
        {
            hitCollider.offset = new Vector2(0, -1);
            hitCollider.size = new Vector2(0.6f, 1f);
            animator.SetFloat("LastDirectionX", 0);
            animator.SetFloat("LastDirectionY", -1);
            playerLastDirection = Vector3.down;
            grappleOffsetY = -0.5f;
            grappleOffsetX = 0f;

        }

        if (Input.GetKey(KeyCode.D) && !actionOnGoing)
        {
            hitCollider.offset = new Vector2(1, 0);
            hitCollider.size = new Vector2(1f, 0.6f);
            animator.SetFloat("LastDirectionX", 1);
            animator.SetFloat("LastDirectionY", 0);
            playerLastDirection = Vector3.right;
            grappleOffsetX = 0.5f;
            grappleOffsetY = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !actionOnGoing)
        {
            actionOnGoing = true;
            animator.SetTrigger("Attack");

        }

    }


    private IEnumerator damageFlash()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

    }


    public void takeDamage (int damage)
    {
        health -= damage;
        HealthUpdate();
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            PlayerDeath();
        }
        else{StartCoroutine(damageFlash());}
      
    }

    public void PlayerDeath()
    {
        canvas.enabled = false;
        playerAlive = false;
        deathBackground.gameObject.SetActive(true);
        gameObject.tag = "Untagged";
        this.enabled = false;
        StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(4);
        float elapsedTime = 0f;
       Color startColor = gameOverText.color;
        

        while (elapsedTime < 3f)
        {
            float alphaChannel = Mathf.Lerp(0f, 1f, elapsedTime / 3f);
            gameOverText.color = new Color(startColor.r, startColor.g, startColor.b, alphaChannel);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        restartButton.gameObject.SetActive(true);

    }


    public void restartGame()
    {
        SceneManager.LoadScene(currentScene);
    }

    public void HealthUpdate()
    {
        switch (health)
        {

            case 6:
                hearts[2].sprite = heartTextures[fullHeart];
                break;
            case 5:
                hearts[2].sprite = heartTextures[halfHeart];
                break;
            case 4:
                hearts[1].sprite = heartTextures[fullHeart];
                hearts[2].sprite = heartTextures[emptyHeart];
                break;
            case 3:
                hearts[1].sprite = heartTextures[halfHeart];
                break;
            case 2:
                hearts[0].sprite = heartTextures[fullHeart];
                hearts[1].sprite = heartTextures[emptyHeart];
                break;
            case 1:
                hearts[0].sprite = heartTextures[halfHeart];
                break;
            case 0:
                hearts[0].sprite = heartTextures[emptyHeart];
                break;
            default:
                break;


        }
        if (health > maxHealth && GameManager.iHealth == false) 
        {
            health = maxHealth;
        }
    }

    private void Attack()
    {
        hitDetector.GetComponent<BoxCollider2D>().enabled = true;
        rb.velocity = Vector2.zero;
    }

    private void animEnd()
    {
        hitDetector.GetComponent<BoxCollider2D>().enabled = false;
        actionOnGoing = false;
    }


    private IEnumerator GrappleHook(Vector3 playerLastDirection)
    {
        playerFallPit.SetActive(false);
        waterPush.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position,playerLastDirection, grappleRange);
        float checkDistance = hit.distance - 1;
        if (hit.collider != null && grappleEnabled)
        {
           
            if (hit.transform.CompareTag("GrappleableWall") || hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Boss"))
            {
               
                actionOnGoing = true;
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, transform.position);
                float grappleShootElapsedTime = 0f;
                float elapsedTime = 0f;
                float moveTime = hit.distance / 10;
                origPos = transform.position;
                targetPos = hit.point - new Vector2(grappleOffsetX, grappleOffsetY);
                rope.enabled = true;
                while (grappleShootElapsedTime < 0.5f)
                {
                    rope.SetPosition(1, Vector3.Lerp(rope.GetPosition(1), hit.point, curve.Evaluate(grappleShootElapsedTime / (moveTime * 2)))); // (1 / 10) = 0.1
                    grappleShootElapsedTime += Time.deltaTime;
                    yield return null;
                }
                rope.SetPosition(1, hit.point);
                while (elapsedTime < 0.5f)
                {
                    transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / moveTime * 3f);
                    rope.SetPosition(0, transform.position);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                RaycastHit2D pitCheck = Physics2D.Raycast(transform.position, playerLastDirection, 0);




                rope.enabled = false;
                transform.position = targetPos;
                actionOnGoing = false;

                playerFallPit.SetActive(true);
                waterPush.SetActive(true);
            }
        }
        else if (grappleEnabled)
        {
            actionOnGoing = true;

            rope.SetPosition(1, transform.position);
            rope.SetPosition(0, transform.position);
            float elapsedTime = 0f;
            origPos = transform.position;
            rope.enabled = true;
            
            while (elapsedTime < 0.5f)
            {
                rope.SetPosition(1, Vector3.Lerp(rope.GetPosition(1), transform.position + (playerLastDirection * grappleRange) ,elapsedTime / 2.1f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            yield return new WaitForSeconds(0.1f);
            while (elapsedTime < grappleShootMoveTime)
            {
                rope.SetPosition(1, Vector3.Lerp(rope.GetPosition(1), transform.position ,elapsedTime / 2.1f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rope.SetPosition(1, transform.position);
            rope.SetPosition(0, transform.position);
            rope.enabled = false;
            actionOnGoing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            string itemType = collision.gameObject.GetComponent<Collectable>().itemType;
            GameManager.instance.ScoreUpdate(itemType);
            Destroy(collision.gameObject);
        }
    }
}
