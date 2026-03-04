using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("게임 상태")]
    [SerializeField] private int currentStage = 1;
    [SerializeField] private int lives = 3;
    [SerializeField] private int score = 0;

    [Header("씬 이름")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string endingScene = "Ending";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 점수
    public void AddScore(int amount)
    {
        score += amount;
    }

    public int GetScore() => score;

    // 목숨
    public int GetLives() => lives;

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
            GameOver();
    }

    // 스테이지
    public int GetCurrentStage() => currentStage;

    public void StageClear()
    {
        if (currentStage >= 5)
        {
            LoadEnding();
            return;
        }

        currentStage++;
        SceneManager.LoadScene($"Stage{currentStage}");
    }

    public void GameOver()
    {
        currentStage = 1;
        lives = 3;
        score = 0;
        SceneManager.LoadScene(mainMenuScene);
    }

    // 씬 전환
    public void LoadMainMenu()
    {
        currentStage = 1;
        lives = 3;
        score = 0;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadEnding()
    {
        SceneManager.LoadScene(endingScene);
    }

    public void LoadStage(int stage)
    {
        currentStage = stage;
        SceneManager.LoadScene($"Stage{currentStage}");
    }
}