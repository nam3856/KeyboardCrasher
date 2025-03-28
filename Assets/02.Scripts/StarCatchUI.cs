using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public enum SuccessRate
{
    Perfect,
    Success,
    WellDone,
    Tried,
    Bad
}

public class StarCatchUI : MonoBehaviour
{

    public static StarCatchUI Instance;

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

    public RectTransform SuccessZone;
    public event Action<SuccessRate> OnStarCatchCompleted;

    private bool goingRight = true;
    private bool isPlaying = false;
    public SuccessRate Howmuch;
    public TextMeshProUGUI starcatchtimertext;
    public ScreenFlashEffect Volume;
    public CameraZoomFeedback cameraZoomFeedback;
    public GameObject Particle;
    public Animator PlayerAnimator;
    public AudioClip clip;
    public AudioSource source;

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
    private void Start()
    {
        UI_Game.Instance.OnTimerEnd += TimerEnd;
    }
    public void TimerEnd()
    {
        StartCoroutine(StartStarCatch());
    }

    private IEnumerator StartStarCatch()
    {
        Time.timeScale = 0.3f;

        x = UnityEngine.Random.Range(0, 1100);
        SuccessZone.anchoredPosition = new Vector2(x, -253);

        TriedMinX = x;
        WellDoneMinX = x + 200f;
        SuccessMinX = x + 400f;
        PerfectMinX = x + 445f;
        PerfectMaxX = PerfectMinX + 10;
        SuccessMaxX = SuccessMinX + 100;
        WellDoneMaxX = WellDoneMinX + 500;
        TriedMaxX = TriedMinX + 900;

        canvas.DOFade(1f, 1f);
        Volume.VignetteStart().Forget();
        
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

            isPlaying = false;
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        GetComponent<CanvasGroup>().alpha = 0f;
        FlyGo().Forget();
    }

    public async UniTaskVoid FlyGo()
    {
        Time.timeScale = 1f;
        Volume.VignetteStart(0, 0.01f).Forget();
        if (Howmuch >= SuccessRate.Success)
        {
            Volume.FlashBloom(100f, 0.3f).Forget();

            cameraZoomFeedback.SmoothZoomEffect(30, 0.3f).Forget();
        }
        else
        {
            cameraZoomFeedback.SmoothZoomEffect(50, 0.3f).Forget();
        }

        source.PlayOneShot(clip);
        await UniTask.Delay(290);
        PlayerAnimator.SetInteger("rand", 2);
        PlayerAnimator.SetTrigger("Attack");
        SoundStageController.Instance.PlayEffectSound();
        await UniTask.Delay(60);

        OnStarCatchCompleted?.Invoke(Howmuch);
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

            isPlaying = false;

            HideUI().Forget();
        }
    }

    private async UniTaskVoid HideUI()
    {
        Particle.SetActive(true);
        await UniTask.WaitForSeconds(0.98f, true);
        GetComponent<CanvasGroup>().alpha = 0f;
    }
}
