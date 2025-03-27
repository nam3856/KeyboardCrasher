using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading.Tasks.Sources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public static UI_Game Instance;
    private Tween _tween;
    public float Timer = 10;
    public Image TimerBar;

    public TextMeshProUGUI ComboText;
    public int Count;
    public GameObject CriticalUI;
    public Button InstructionButton;

    public Button StartButton;

    public GameObject CriticalText;

    public TextMeshProUGUI TimerText;


    public TextMeshProUGUI StartTimeText;
    public TextMeshProUGUI StartInstructionText;
    public TextMeshProUGUI StartTimerText;

    public Canvas StarCatchUI;

    public event Action OnTimerEnd;
    public event Action OnTimerStart;
    public GameObject sandBag;

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

        StartButton.onClick.AddListener(StartTimer);
    }

    private void Start()
    {
        global::StarCatchUI.Instance.OnStarCatchCompleted += StartChase;
    }

    public void StartChase()
    {
        ChaseX().Forget();
    }

    public async UniTaskVoid ChaseX()
    {
        while (sandBag.GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            ComboText.text = $"{sandBag.transform.position.x + 4.726f:F2}M";
            await UniTask.Yield();
        }
    }


    public void Add()
    {
        Count++;
        float timeRatio = Timer / 10f;
        float threshold = Mathf.Lerp(100f, 5f, timeRatio);
        ComboText.fontSize = Mathf.Clamp((float)Count / 2, 40f, 120f);
        ComboText.text = Count >= threshold ? $"Combo x {Count}!" : $"Combo x {Count}";

        if (_tween != null && _tween.active)
        {
            _tween.Kill();
        }

        _tween = ComboText.transform.DOScale(3f, 0.1f)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                ComboText.transform.DOScale(1f, 0.1f);
            });
    }

    public void StartTimer()
    {
        StartButton.gameObject.SetActive(false);
        Timer = 10;
        TimerUniTask().Forget();
        SoundStageController.Instance.StartIntroBGM();
    }

    public async UniTaskVoid TimerUniTask()
    {
        StartTimerText.gameObject.SetActive(true);
        StartTimeText.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(1);
        StartInstructionText.gameObject.SetActive(true);
        sandBag.GetComponent<Animator>().SetTrigger("start");
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "2";
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "1";
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "START!";

        OnTimerStart?.Invoke();
        StartTimeText.transform.DOScale(2f, 0.01f).OnComplete(() => StartTimeText.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            StartTimeText.gameObject.SetActive(false);
        }));

        StartTimerText.gameObject.SetActive(false);
        while (Timer > 0)
        {
            TimerText.text = Timer.ToString("F3");
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 10);
            TimerBar.fillAmount = Timer / 10;
            await UniTask.Yield();
        }

        StartInstructionText.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(false);
        OnTimerEnd?.Invoke();
        StarCatchGo();
    }

    public void StarCatchGo()
    {
        StarForceTimer().Forget();
    }

    public async UniTaskVoid StarForceTimer()
    {
        StarCatchUI.gameObject.SetActive(true);
        Timer = 6f;

        while (Timer > 0)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 6f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            await UniTask.Yield();
        }
    }

    public void ShowCriticalText()
    {
        Count++;
        Instantiate(CriticalText, CriticalUI.transform);
    }
}
