using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public const float GAME_TIME_SCALE = 1.5f;
    
    [SerializeField] private int gameTime = 60;

    private int currentTime;
    private int collectedCoins;
    private int totalCoins;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        currentTime = gameTime;
        Time.timeScale = 0;
        totalCoins = 0;
    }

    private void Update()
    {
        UIManager.Instance.SetTimeLeft(currentTime);
        UIManager.Instance.UpdateCurrentCoinsText(collectedCoins, totalCoins);
    }

    public void StartGame()
    {
        UIManager.Instance.SetActiveStartButton(false);
        currentTime = gameTime;
        StartCoroutine(PassTimeCoroutine());
        Time.timeScale = GAME_TIME_SCALE;
        collectedCoins = 0;
    }

    public void RestartGame()
    {
        StartGame();
        BallBehaviour.Instance.Respawn();
        UIManager.Instance.ShowGameOver(false);
        MazeRotateScript.Instance.ResetAngle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CollectCoin() => collectedCoins++;

    public void AddCointToTotal() => totalCoins++;

    private void GameOver()
    {
        Time.timeScale = 0;
        UIManager.Instance.UpdateCollectedCoinsText(collectedCoins, totalCoins);
        UIManager.Instance.ShowGameOver(true);
        //BluetoothManager.Instance.SendBluetoothMessage(BluetoothManager.GAME_OVER_MSG_CODE);
    }

    private IEnumerator PassTimeCoroutine()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(GAME_TIME_SCALE);
            currentTime--;
        }
        GameOver();
    }
}
