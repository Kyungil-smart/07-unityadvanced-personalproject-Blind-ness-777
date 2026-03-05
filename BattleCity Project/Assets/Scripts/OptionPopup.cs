using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionPopup : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject exitButton;

    // 활성화 시 볼륨 리스너 등록 및 씬 전환 이벤트 구독
    private void OnEnable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        SceneManager.sceneLoaded += OnSceneLoaded;
        RefreshUI();
    }

    // 비활성화 시 리스너 해제. 메모리 누수 방지
    private void OnDisable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 전환 후 Exit 버튼 표시 상태 갱신 및 timeScale 복구
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUI();
        if (!IsInGame())
            Time.timeScale = 1f;
    }

    // 인게임에서만 ESC로 팝업 토글
    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame &&
            IsInGame())
        {
            TogglePopup();
        }
    }

    // 인게임이면 Exit 버튼 표시, 메인메뉴면 숨김
    private void RefreshUI()
    {
        if (exitButton != null)
            exitButton.SetActive(IsInGame());
    }

    // 메인메뉴/인게임 공용. 인게임에서는 팝업 열릴 때 일시정지
    public void TogglePopup()
    {
        if (popupPanel == null) return;
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
        if (popupPanel != null)
            popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // 인게임에서는 메인메뉴로 복귀, 메인메뉴에서는 게임 종료
    public void OnExitButton()
    {
        if (IsInGame())
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