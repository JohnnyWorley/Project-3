using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("UI Elements")]

    public GameObject pauseMenu;


    public Button gemButton;
    private bool iGems = false;
    private int prevGems;
    
    public Button healthButton;
    public static bool iHealth = false;
    private float prevHealth;


    public TextMeshProUGUI scoreCounter;
    private PlayerController playerController;
    private HitDetection hitDetection;
    private int score;
    public static bool isPaused = false;
    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        hitDetection = FindObjectOfType<HitDetection>();
        playerController = FindObjectOfType<PlayerController>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused && playerController.playerAlive)
        {
            //Unpause
            pauseMenu.SetActive(false);
            isPaused = false;
            playerController.actionOnGoing = false;
            Time.timeScale = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && playerController.playerAlive)
        {
            //Pause
            pauseMenu.SetActive(true);
            playerController.actionOnGoing = true;
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    public void ScoreUpdate(string itemType)
    {
        switch (itemType)
        {
            case "BlueGem":
                playerController.gems += 1;
                break;
            case "GreenGem":
                playerController.gems += 3;
                break;
            case "Heart":
                playerController.health++;
                playerController.HealthUpdate();
                break;
            default: 
                break;
        }

        scoreCounter.text =  ("x"+playerController.gems.ToString());
    }

    public void itemBuy(string itemType)
    {
        switch (itemType)
        {
            case "Spinach":
                hitDetection.critChance = hitDetection.critChance / 2;
                break;
            case "swordUpgrade":
                hitDetection.damage *= 1.5f;
                break;
            case "heartBuy":
                playerController.health ++;
                playerController.HealthUpdate();
                playerController.health++;
                playerController.HealthUpdate();
                break;
        }
        scoreCounter.text = ("x" + playerController.gems.ToString());
    }


    public void InfiniteGems()
    {
        if (!iGems)
        {
            gemButton.GetComponent<Image>().color = Color.green;
            playerController.gems = 9999;
            iGems = true;
            scoreCounter.text = ("x" + playerController.gems.ToString());
        }
       else if (iGems)
        {
            gemButton.GetComponent<Image>().color = new Color(144,32,32);
            playerController.gems = 0;
            iGems = false;
            scoreCounter.text = ("x" + playerController.gems.ToString());
        }
    }


    public void InfiniteHealth()
    {

        if (!iHealth)
        {
            healthButton.GetComponent<Image>().color = Color.green;
            playerController.health = 99999999;
            iHealth = true;
            playerController.HealthUpdate();

        }
        else if (iHealth)
        {
            healthButton.GetComponent<Image>().color = new Color(144, 32, 32);
            playerController.health = 6;
            iHealth = false;
            playerController.HealthUpdate();
        }
    }

}
