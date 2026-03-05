using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPopup;

    public void OnStartButton()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnOptionButton()
    {
        if (optionPopup != null)
            optionPopup.GetComponent<OptionPopup>().TogglePopup();
    }

    // GameManager가 없으면 직접 씬 이동 (메인메뉴에 GameManager 없음)
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