using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI stageNumText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Lives")]
    [SerializeField] private RawImage[] lifeIcons; // 3개 연결
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateStage();
        UpdateScore();
        UpdateLives();
    }

    public void UpdateStage()
    {
        if (stageNumText != null)
            stageNumText.text = $"STAGE {GameManager.Instance?.GetCurrentStage()}";
    }

    public void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = $"SCORE {GameManager.Instance?.GetScore()}";
    }

    public void UpdateEnemyCount(int count)
    {
        if (enemyCountText != null)
            enemyCountText.text = $"ENEMY {count}";
    }

    public void UpdateLives()
    {
        if (lifeIcons == null) return;
        int lives = GameManager.Instance?.GetLives() ?? 0;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
                lifeIcons[i].gameObject.SetActive(i < lives);
        }
    }
}