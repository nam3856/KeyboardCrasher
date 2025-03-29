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

        // 1. ���̵� �ƿ� (�˰�)
        fadeImage.DOFade(1f, 0.5f).OnComplete(() =>
        {
            // 2. �� �ε�
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // 3. �� ������ ������ �� ���̵� �� ����
            StartCoroutine(DelayedFadeIn());
        });
    }

    private System.Collections.IEnumerator DelayedFadeIn()
    {
        yield return null; // ���� �ε�ǰ� �� ������ ��ٸ�

        fadeImage.DOFade(0f, 0.5f).OnComplete(() =>
        {
            fadeImage.raycastTarget = false;
        });
    }
}
