using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

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