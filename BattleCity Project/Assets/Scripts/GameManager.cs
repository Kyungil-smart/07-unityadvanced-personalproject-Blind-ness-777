using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("게임 상태")]
    [SerializeField] private int currentStage = 1;
    [SerializeField] private int lives = 3;
    [SerializeField] private int score = 0;
    public bool isEnding = false;

    [Header("씬 이름")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string endingScene = "EndingCredit";

    // 씬 전환 후에도 상태 유지
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

    public void AddScore(int amount)
    {
        score += amount;
        FindObjectOfType<GameUIManager>()?.UpdateScore();
    }

    public int GetScore() => score;
    public int GetLives() => lives;
    public int GetCurrentStage() => currentStage;

    // 목숨 차감. 0 이하면 게임오버
    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
            GameOver();
    }

    // 5스테이지 클리어 시 엔딩, 그 외엔 다음 스테이지로
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
        SceneManager.LoadScene("GameOver");
    }

    // 메인메뉴로 돌아갈 때 모든 상태 초기화
    public void LoadMainMenu()
    {
        currentStage = 1;
        lives = 3;
        score = 0;
        SceneManager.LoadScene(mainMenuScene);
    }

    // isEnding 플래그로 EndingSceneManager가 엔딩/크레딧 분기 처리
    public void LoadEnding()
    {
        isEnding = true;
        SceneManager.LoadScene(endingScene);
    }

    public void LoadStage(int stage)
    {
        currentStage = stage;
        SceneManager.LoadScene($"Stage{currentStage}");
    }
}