using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
public enum SuccessRate
{
    Perfect,
    Success,
    WellDone,
    Tried,
    Bad
}
public class StarCatchBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform pointer;
    [SerializeField] private RectTransform bar;
    [SerializeField] private float speed = 600f;
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

    public float multipler = 0.1f;
    public float[] multiplers;
    public RectTransform SuccessZone;
    public event Action OnStarCatchCompleted;

    private bool goingRight = true;
    private bool isPlaying = false;
    public static StarCatchBarUI Instance;
    public SuccessRate Howmuch;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {

    }

    public void TimerEnd()
    {
        StartCoroutine(StartStarCatch());
    }
    private IEnumerator StartStarCatch()
    {
        Time.timeScale = 0.3f;
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

        canvas.DOFade(1f, 1f);
        yield return new WaitForSeconds(3);
        isPlaying = true;
    }

    public void ForceStarCatchEnd()
    {
        if (isPlaying)
        {
            float x = pointer.anchoredPosition.x;
            if (x >= PerfectMinX && x <= PerfectMaxX)
            {
                Howmuch = SuccessRate.Perfect;
            }
            else if (x >= SuccessMinX && x <= SuccessMaxX)
            {
                Howmuch = SuccessRate.Success;
            }
            else if (x >= WellDoneMinX && x <= WellDoneMaxX)
            {
                Howmuch = SuccessRate.WellDone;
            }
            else if (x >= TriedMinX && x <= TriedMaxX)
            {
                Howmuch = SuccessRate.Tried;
            }
            else
            {
                Howmuch = SuccessRate.Bad;
            }


            multipler = multiplers[(int)Howmuch];
            isPlaying = false;

            GetComponent<CanvasGroup>().alpha = 0f;
        }
        Debug.Log(Howmuch);
        OnStarCatchCompleted?.Invoke();
    }

    void Update()
    {
        if (!isPlaying) return;

        // 좌우 움직임
        float move = speed * Time.deltaTime * (goingRight ? 1 : -1);
        pointer.anchoredPosition += new Vector2(move, 0);

        // 양쪽 끝 반전
        if (pointer.anchoredPosition.x >= bar.rect.width)
            goingRight = false;
        else if (pointer.anchoredPosition.x <= 0)
            goingRight = true;

        // 입력 판정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float x = pointer.anchoredPosition.x;
            if (x >= PerfectMinX && x <= PerfectMaxX)
            {
                Howmuch = SuccessRate.Perfect;
            }
            else if (x >= SuccessMinX && x <= SuccessMaxX)
            {
                Howmuch = SuccessRate.Success;
            }
            else if (x >= WellDoneMinX && x <= WellDoneMaxX)
            {
                Howmuch = SuccessRate.WellDone;
            }
            else if (x >= TriedMinX && x <= TriedMaxX)
            {
                Howmuch = SuccessRate.Tried;
            }
            else
            {
                Howmuch = SuccessRate.Bad;
            }


            multipler = multiplers[(int)Howmuch];
            isPlaying = false;

            Debug.Log(Howmuch);
            GetComponent<CanvasGroup>().alpha = 0f;
        }
    }
}
