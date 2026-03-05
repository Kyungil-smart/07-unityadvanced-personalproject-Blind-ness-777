using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionPopup : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    
    [Header("OptionPopUp")]
    [SerializeField] private GameObject popupPanel;

    [Header("인게임 전용 버튼")]
    [SerializeField] private GameObject mainMenuButton;

    private void Awake()
    {
        // 인게임 씬에서만 메인메뉴 버튼 활성화
        bool isInGame = SceneManager.GetActiveScene().name.StartsWith("Stage");
        if (mainMenuButton != null)
            mainMenuButton.SetActive(isInGame);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isInGame = SceneManager.GetActiveScene().name.StartsWith("Stage");
            if (isInGame)
                TogglePopup();
        }
    }

    public void TogglePopup()
    {
        bool isInGame = SceneManager.GetActiveScene().name.StartsWith("Stage");
    
        popupPanel.SetActive(!popupPanel.activeSelf);
    
        if (isInGame)
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

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.LoadMainMenu();
    }
}