using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private Text timeLeftText;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private Text collectedCoinsText;
    [SerializeField] private Text currentCollectedCoinsText;
    
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

    public void SetTimeLeft(int time)
    {
        timeLeftText.text = $"Time Left: {time}";
    }

    public void UpdateCollectedCoinsText(int coins, int total)
    {
        collectedCoinsText.text = $"Coins collected: {coins}/{total}";
    }

    public void UpdateCurrentCoinsText(int coins, int total)
    {
        currentCollectedCoinsText.text = $"Coins collected: {coins}/{total}";
    }

    public void ShowGameOver(bool status)
    {
        gameOverPanel.SetActive(status);
        hudPanel.SetActive(!status);
    }
    
    public void SetActiveStartButton(bool status)
    {
        startButton.gameObject.SetActive(status);
    }
}
