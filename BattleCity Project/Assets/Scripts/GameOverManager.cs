using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    // 게임오버 시점의 점수 표시
    private void Start()
    {
        if (scoreText != null)
            scoreText.text = $"SCORE {GameManager.Instance?.GetScore()}";
    }

    public void OnMainMenuButton()
    {
        GameManager.Instance?.LoadMainMenu();
    }
}