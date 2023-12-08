using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Takes care of the scoring system.
/// 
/// </summary>
public class GameManager : MonoBehaviour
{

    private static GameManager instance; // Singleton instance

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    instance = singletonObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    
    public GameObject gameOver;
    public int coins = 0;
    public TMP_Text coinText;

    public int life = 6;
    public Image lifeImage;

    public int oportunities = 3;
    public TMP_Text oportunitiesText;
    
    public GameObject starPrefab;
    public GameObject star;
    private Coroutine hideCoroutine;


    private void Awake()
    {
        star = starPrefab;
    }

    private void Start()
    { 
        coins = 0;
        life = 8;
        
        UpdateCoinsUI();
        UpdateLifeUI();
        UpdateOportunitiesUI();

    }

    public void AddCoins(int amount)
    {
        coins += amount;
        
        UpdateCoinsUI();
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;

        if (coins < 0)
            coins = 0;
        
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        coinText.SetText(coins.ToString());
    }

    public void GetStar()
    {
        if (life < 8)
        {
            life = Math.Min(life+2, 8);
            UpdateLifeUI();
        }
        else
        {
            oportunities++;
            UpdateOportunitiesUI();
        }
    }
    
    public void AddLife(int amount)
    {
        
        life += amount;
        if (life > 6) life = 6;
        
        UpdateLifeUI();
    }

    public void RemoveLife(int amount)
    {
        life -= amount;
        if (life <= 0)
        {
            life = 0;
            Die();
        }
        
        UpdateLifeUI();
    }
    
    public void FillLife()
    {
        life = 8;
        UpdateLifeUI();
    }

    public void UpdateLifeUI()
    {
        lifeImage.transform.parent.gameObject.SetActive(true);
    
        //make lifeimage slice based on life/8 (8 is maxlife)
        //also make color of lifeimage based on life (8-7 green, 6-5 yellowish green, 4-3 yellow, 1-2 red)
        
        // Calculate the fill amount based on the current life
        float fillAmount = life / 8f; // Assuming max life is 6

        // Update the fill amount of the lifeImage
        lifeImage.fillAmount = fillAmount;

        // Set color based on life range
        Color lifeColor;
        if (life >= 7)
        {
            lifeColor = Color.blue;
        }
        else if (life >= 5)
        {
            lifeColor = Color.green;
        }
        else if (life >= 3)
        {
            lifeColor = Color.yellow;
        }
        else
        {
            lifeColor = Color.red;
        }

        // Apply the color to the lifeImage
        lifeImage.color = lifeColor;
        
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // Show the lifeImage's parent for 3 seconds
        hideCoroutine = StartCoroutine(HideLifeAfterDelay(3f));
    }

    private IEnumerator HideLifeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the lifeImage after the delay
        lifeImage.transform.parent.gameObject.SetActive(false);
    }
    
    void UpdateOportunitiesUI()
    {
        oportunitiesText.SetText(oportunities.ToString());
    }

    public void RemoveOportunity()
    {
        oportunities -= 1;
        oportunitiesText.SetText(oportunities.ToString());
        
    }

    public void Die()
    {
        gameOver.SetActive(true);
        RemoveOportunity();
        FillLife();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void SpawnStar()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 starPos = player.position;
        starPos.y += 3f;

        Instantiate(star, starPos, player.rotation);
    }
}
