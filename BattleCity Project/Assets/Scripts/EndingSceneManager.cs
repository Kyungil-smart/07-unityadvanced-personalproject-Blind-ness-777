using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private GameObject creditContent;

    [Header("Credit Scroll")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float endingDisplayTime = 3f;
    [SerializeField] private float scrollStopY = 1000f;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Press Enter")]
    [SerializeField] private GameObject pressEnterText;

    private bool isScrolling = false;
    private float timer = 0f;
    private float fadeTimer = 0f;
    private bool isFading = false;

    // GameManager.isEnding 플래그로 엔딩/크레딧 분기
    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.isEnding)
        {
            endingPanel.SetActive(true);
            creditContent.SetActive(false);
        }
        else
        {
            // 메인메뉴 크레딧 버튼으로 진입 시 크레딧만 표시
            endingPanel.SetActive(false);
            creditContent.SetActive(true);
            isScrolling = true;
        }
    }

    private void Update()
    {
        // ESC/Enter로 언제든 메인메뉴 복귀
        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            OnMainMenuButton();
            return;
        }

        HandleEndingFade();
        HandleCreditScroll();
    }

    // 엔딩 이미지를 일정 시간 표시 후 페이드 아웃
    private void HandleEndingFade()
    {
        if (endingPanel.activeSelf && !isFading)
        {
            timer += Time.deltaTime;
            if (timer >= endingDisplayTime)
            {
                isFading = true;
                fadeTimer = 0f;
            }
        }

        if (isFading && fadeCanvasGroup != null)
        {
            fadeTimer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            if (fadeTimer >= fadeDuration)
            {
                endingPanel.SetActive(false);
                creditContent.SetActive(true);
                isScrolling = true;
                isFading = false;
                fadeCanvasGroup.alpha = 1f;
            }
        }
    }

    // 크레딧 스크롤. scrollStopY 도달 시 페이드 아웃 후 PressEnter 표시
    private void HandleCreditScroll()
    {
        if (!isScrolling || creditContent == null) return;

        if (creditContent.transform.localPosition.y < scrollStopY)
        {
            creditContent.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
        else
        {
            isScrolling = false;
            StartCoroutine(FadeOutCredit());
        }
    }

    public void OnMainMenuButton()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.isEnding = false;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator FadeOutCredit()
    {
        float elapsed = 0f;
        CanvasGroup cg = creditContent.GetComponent<CanvasGroup>();
        if (cg == null) cg = creditContent.AddComponent<CanvasGroup>();

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        if (pressEnterText != null)
            pressEnterText.SetActive(true);
    }
}