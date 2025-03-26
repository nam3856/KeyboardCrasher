using DG.Tweening;
using TMPro;
using UnityEngine;

public class Critical : MonoBehaviour
{
    public RectTransform RT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randX = Random.Range(-0.3f, 0.3f);
        float randY = Random.Range(-0.3f, 0.3f);
        RT.anchoredPosition = new Vector2(randX, randY);
        transform.DOScale(0.08f, 0.05f)
            .OnComplete(() => transform.DOScale(0.02f, 0.1f).SetEase(Ease.OutExpo));

        float y = RT.anchoredPosition.y + 2;
        float r = Random.Range(1.1f, 2f);
        RT.DOAnchorPosY(y, r).OnComplete(() => Destroy(gameObject));
        GetComponent<TextMeshProUGUI>().DOFade(0f, r);
    }

}
