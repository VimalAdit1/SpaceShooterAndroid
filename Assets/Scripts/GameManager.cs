using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private const float spawnRadius = 50f;
    public GameObject player;
    public GameObject[] obstacles;
    public GameObject powerUp;
    public GameObject gameOverScreen;
    public GameObject pauseMenu;
    public Text scoreText;
    public Text coinsText;
    public Text GameOverText;
    public Text highScoreText;
    public Text GameOverHiText;
    public int noOfObstacles=0;
    public int noOfBGElements=0;
    public int selectedGun;
    public float minSpeed = 3f;
    public float maxSpeed = 7f;
    public int minObstacles = 5;
    public float speed;
    int score;
    int selected = 0;
    public float nextPowerup;
    public bool spawnPowerup;
    int coins;
    public Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        nextPowerup = 0;
        spawnPowerup = true;
        gameOverScreen.SetActive(false);
        score = 0;
        scoreText.text = "0";
        highScoreText.text=highScore().ToString();
        speed = minSpeed;
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        if(noOfObstacles<0)
        {
            noOfObstacles = 0;
        }
        if(noOfObstacles<minObstacles)
        {
            spawnObstacle();
        }
        if (spawnPowerup&&Time.time>nextPowerup)
        {
            spawnPowerUp();
            spawnPowerup = false;
        }
       
    }

    private void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.active)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (gameOverScreen.active)
            {
                Retry();
            }
        }
    }

    //GameLoop
    private void spawnPowerUp()
    {
            Vector3 point = (UnityEngine.Random.insideUnitSphere * spawnRadius) + player.transform.position;
            point.z = 0f;
            GameObject obstacle = Instantiate(powerUp, point, Quaternion.identity);
            PowerUp p = obstacle.GetComponent<PowerUp>();
            p.player = player;
            p.gameManager = this;
            int gun;
            do
            {
                gun = UnityEngine.Random.Range(0, 3);
            } while (gun == selectedGun);
            p.gunNo = gun;
            p.GetComponent<SpriteRenderer>().color = colors[gun];
    }

    private void spawnObstacle()
    {
        Vector3 point = (UnityEngine.Random.insideUnitSphere * spawnRadius) + player.transform.position;
        point.z = 0f;
        GameObject obstacle = Instantiate(obstacles[0], point,Quaternion.identity);
        Enemy e = obstacle.GetComponentInChildren<Enemy>();
        e.player = player;
        e.speed = speed;
        e.gameManager = this;
        noOfObstacles++;
    }
    //Score
    public void AddScore(int points)
    {
        score+=points;
        scoreText.text = score.ToString();
        highScoreText.text = highScore().ToString();
        if(speed<maxSpeed)
        {
            speed += 0.1f;
        }
        if(score%100==0)
        {
            minObstacles++; 
        }
        AddCoins(points/2);
    }

    private void AddCoins(int no)
    {
        coins += no;
        PlayerPrefs.SetInt("Coins", coins);
        coinsText.text = coins.ToString();
    }

    public string GetScore()
    {
        return score==0?"Better Luck Next Time":"You Scored:"+score.ToString();
    }
    public string GetHighScore()
    {
        string message="";
        if(highScore()==score)
        {
            message="New!!!";
        }
        return message+"High Score:"+highScore().ToString();
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0.5f;
        GameOverText.text = GetScore();
        GameOverHiText.text = GetHighScore();
    }
    public int highScore()
    {
        int highScore = PlayerPrefs.GetInt("highScore",0);
        if(highScore<score)
        {
            PlayerPrefs.SetInt("highScore",score);
            highScore=score;
        }
        return highScore;
    }
    //UI
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
   
}
