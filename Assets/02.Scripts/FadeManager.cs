using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartSceneWithFade()
    {
        fadeImage.raycastTarget = true;

        // 1. 페이드 아웃 (검게)
        fadeImage.DOFade(1f, 0.5f).OnComplete(() =>
        {
            // 2. 씬 로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // 3. 한 프레임 딜레이 후 페이드 인 시작
            StartCoroutine(DelayedFadeIn());
        });
    }

    private System.Collections.IEnumerator DelayedFadeIn()
    {
        yield return null; // 씬이 로드되고 한 프레임 기다림

        fadeImage.DOFade(0f, 0.5f).OnComplete(() =>
        {
            fadeImage.raycastTarget = false;
        });
    }
}
