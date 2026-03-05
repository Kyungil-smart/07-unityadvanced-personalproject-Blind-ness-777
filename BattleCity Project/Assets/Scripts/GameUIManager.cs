using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI stageNumText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Lives")]
    [SerializeField] private RawImage[] lifeIcons;

    // 씬 로드 시 현재 GameManager 상태로 UI 초기화
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

    // SpawnManager에서 적 스폰/사망 시 호출
    public void UpdateEnemyCount(int count)
    {
        if (enemyCountText != null)
            enemyCountText.text = $"ENEMY {count}";
    }

    // 목숨 수만큼 아이콘 활성화
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