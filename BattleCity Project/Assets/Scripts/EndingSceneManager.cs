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
    [SerializeField] private float scrollStopY = 1000f; // 추가

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    
    [Header("Press Enter")]
    [SerializeField] private GameObject pressEnterText;

    private bool isCreditDone = false;
    private bool isScrolling = false;
    private float timer = 0f;
    private float fadeTimer = 0f;
    private bool isFading = false;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.isEnding)
        {
            // 엔딩 이미지 먼저 보여주기
            endingPanel.SetActive(true);
            creditContent.SetActive(false);
        }
        else
        {
            // 메인메뉴에서 크레딧 버튼으로 진입
            endingPanel.SetActive(false);
            creditContent.SetActive(true);
            isScrolling = true;
        }
    }

    private void Update()
    {
        // 언제든 탈출 가능
        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            OnMainMenuButton();
            return;
        }
        
        // 엔딩 이미지 표시 후 페이드 아웃
        if (endingPanel.activeSelf && !isFading)
        {
            timer += Time.deltaTime;
            if (timer >= endingDisplayTime)
            {
                isFading = true;
                fadeTimer = 0f;
            }
        }

        // 페이드 처리
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
                if (fadeCanvasGroup != null)
                    fadeCanvasGroup.alpha = 1f;
            }
        }

        // 크레딧 스크롤
        if (isScrolling && creditContent != null)
        {
            if (creditContent.transform.localPosition.y < scrollStopY)
            {
                creditContent.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            }
            else
            {
                isScrolling = false;
                isCreditDone = true;
                StartCoroutine(FadeOutCredit());
            }
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
        float elapsed = 0f; // timer → elapsed
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