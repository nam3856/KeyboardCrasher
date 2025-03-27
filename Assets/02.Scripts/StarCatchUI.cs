using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class StarCatchBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform pointer;
    [SerializeField] private RectTransform bar;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float PerfectMinX;
    [SerializeField] private float PerfectMaxX;
    [SerializeField] private float SuccessMinX;
    [SerializeField] private float SuccessMaxX;
    [SerializeField] private float WellDoneMinX;
    [SerializeField] private float WellDoneMaxX;
    [SerializeField] private float TriedMinX;
    [SerializeField] private float TriedMaxX;
    [SerializeField] private float x;
    [SerializeField] private CanvasGroup canvas;

    public RectTransform SuccessZone;
    public event Action OnStarCatchCompleted;

    private bool goingRight = true;
    private bool isPlaying = true;
    private void OnEnable()
    {
        x = UnityEngine.Random.Range(-100, 1100);
        SuccessZone.anchoredPosition = new Vector2(x, 50);

        TriedMinX = x;
        WellDoneMinX = x + 200f;
        SuccessMinX = x + 400f;
        PerfectMinX = x + 445f;
        PerfectMaxX = PerfectMinX + 10;
        SuccessMaxX = SuccessMinX + 100;
        WellDoneMaxX = WellDoneMinX + 500;
        TriedMaxX = TriedMinX + 900;

        StartCoroutine(StartStarCatch());
    }

    private IEnumerator StartStarCatch()
    {
        canvas.DOFade(1f, 1f);
        yield return new WaitForSeconds(3);
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

        // 좌우 움직임
        float move = speed * Time.deltaTime * (goingRight ? 1 : -1);
        pointer.anchoredPosition += new Vector2(move, 0);

        // 양쪽 끝 반전
        if (pointer.anchoredPosition.x >= bar.rect.width / 2f)
            goingRight = false;
        else if (pointer.anchoredPosition.x <= -bar.rect.width / 2f)
            goingRight = true;

        // 입력 판정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float x = pointer.anchoredPosition.x;
            if (x >= PerfectMinX && x <= PerfectMaxX)
                Debug.Log("Perfect!");
            else if (x >= SuccessMinX && x <= SuccessMaxX)
                Debug.Log("Nice!");
            else if (x >= WellDoneMinX && x <= WellDoneMaxX)
                Debug.Log("Good!");
            else if (x >= TriedMinX && x <= TriedMaxX)
                Debug.Log("Nice Try!");
            else
                Debug.Log("Bad");

            
            isPlaying = false;
            OnStarCatchCompleted?.Invoke();
        }
    }
}
