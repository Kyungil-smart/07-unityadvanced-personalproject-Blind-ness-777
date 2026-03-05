using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionPopup;

    public void OnStartButton()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnOptionButton()
    {
        if (optionPopup != null)
            optionPopup.SetActive(true);
    }

    public void OnCreditButton()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadEnding();
        else
            SceneManager.LoadScene("EndingCredit");
    }

    public void OnQuitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void CloseOption()
    {
        if (optionPopup != null)
            optionPopup.SetActive(false);
    }
}