using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

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
    [SerializeField] private float speed = 1200f;
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

    public CameraZoomFeedback zoomFeedback;
    public float multipler = 0.1f;
    public float[] multiplers;
    public RectTransform SuccessZone;
    public event Action OnStarCatchCompleted;

    private bool goingRight = true;
    private bool isPlaying = false;
    public static StarCatchBarUI Instance;
    public SuccessRate Howmuch;
    public TextMeshProUGUI starcatchtimertext;

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

    public void TimerEnd()
    {
        StartCoroutine(StartStarCatch());
    }

    private IEnumerator StartStarCatch()
    {
        Time.timeScale = 0.3f;

        x = UnityEngine.Random.Range(-100, 1100);
        SuccessZone.anchoredPosition = new Vector2(x, -253);

        TriedMinX = x;
        WellDoneMinX = x + 200f;
        SuccessMinX = x + 400f;
        PerfectMinX = x + 445f;
        PerfectMaxX = PerfectMinX + 10;
        SuccessMaxX = SuccessMinX + 100;
        WellDoneMaxX = WellDoneMinX + 500;
        TriedMaxX = TriedMinX + 900;

        zoomFeedback.DoZoomEffect(14f, 6f);
        canvas.DOFade(1f, 1f);

        yield return new WaitForSecondsRealtime(1f);
        starcatchtimertext.text = "2";

        yield return new WaitForSecondsRealtime(1f);
        starcatchtimertext.text = "1";

        yield return new WaitForSecondsRealtime(1f);
        starcatchtimertext.text = "START!";

        isPlaying = true;

        yield return new WaitForSecondsRealtime(1f);
        starcatchtimertext.text = "";
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

        Time.timeScale = 1f;
        OnStarCatchCompleted?.Invoke();
    }

    private void Update()
    {
        if (!isPlaying) return;

        float move = speed * Time.unscaledDeltaTime * (goingRight ? 1 : -1);
        pointer.anchoredPosition += new Vector2(move, 0);

        if (pointer.anchoredPosition.x >= bar.rect.width)
            goingRight = false;
        else if (pointer.anchoredPosition.x <= 0)
            goingRight = true;

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
