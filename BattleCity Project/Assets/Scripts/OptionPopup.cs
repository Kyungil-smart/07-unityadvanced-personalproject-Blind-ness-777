using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionPopup : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject exitButton;

    private void Awake()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        
        // 메인메뉴에서는 Exit 버튼 숨기기
        if (exitButton != null)
            exitButton.SetActive(IsInGame());
    }

    // 인게임에서만 ESC로 팝업 토글
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && IsInGame())
            TogglePopup();
    }

    // 메인메뉴/인게임 공용. 인게임에서는 팝업 열릴 때 일시정지
    public void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);

        if (IsInGame())
            Time.timeScale = popupPanel.activeSelf ? 0f : 1f;
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnCloseButton()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // 인게임에서 메인메뉴로 복귀 시 timeScale 복구
    public void OnExitButton()
    {
        bool isInGame = IsInGame();

        if (isInGame)
        {
            Time.timeScale = 1f;
            GameManager.Instance?.LoadMainMenu();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private bool IsInGame()
    {
        return SceneManager.GetActiveScene().name.StartsWith("Stage");
    }
}