using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public static UI_Game Instance;
    private Tween _tween;
    public float Timer = 10;
    public Image TimerBar;
    public Image TimerBase;
    public TextMeshProUGUI TimerText;

    public TextMeshProUGUI ComboText;
    public int Count;
    public GameObject CriticalUI;
    public Button InstructionButton;

    public Button StartButton;

    public GameObject CriticalText;

    public GameObject StarCatchTimerParticle;
    public ParticleSystem StarCatchTimerParticleSystem;

    public TextMeshProUGUI StartTimeText;
    public TextMeshProUGUI StartInstructionText;
    public TextMeshProUGUI StartTimerText;

    public Canvas StarCatchUI;

    public event Action OnTimerEnd;
    public event Action OnTimerStart;
    public GameObject sandBag;
    public CameraZoomFeedback CameraZoomFeedback;
    public TextMeshProUGUI TitleText;
    public float BestScore = 0;
    public TextMeshProUGUI NewRecordText;

    public TextMeshProUGUI BestScoreText;

    public async UniTaskVoid UpdateBestScoreAndShowNewRecordText(float bestScore)
    {
        float increase = 0.001f;
        while(BestScore < (int)bestScore)
        {
            BestScore += increase;
            BestScoreText.text = $"{BestScore:F3}M";
            await UniTask.Yield();
            increase *= 1.5f;
        }
        BestScore = bestScore;
        BestScoreText.text = $"{BestScore:F3}M";
        Save();
        RankingManager.Instance.UploadScore(PlayerManager.Instance.playerID, "깽미니", BestScore);
        RankingManager.Instance.GetTopRankings();
        await UniTask.Delay(200);
        NewRecordText.gameObject.SetActive(true);
        NewRecordText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutElastic);
        // TODO: 효과음 추가


    }



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
        string saved = PlayerPrefs.GetString("BestScore");
        Debug.Log(saved);
        if (!string.IsNullOrEmpty(saved))
        {
            Debug.Log(saved);
            if(float.TryParse(saved, out BestScore))
            {
                Debug.Log("변환성공");
                BestScoreText.text = $"{BestScore:F3}M";
            }
            else
            {
                Debug.Log("로드실패");
            }
        }
        else
        {
            Debug.Log("로드 실패 - string is null or empty");
        }
        global::StarCatchUI.Instance.OnStarCatchCompleted += StartChase;
        TitleText.transform.DOScale(0.8f, 0.5f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
        
    }

    public void StartChase()
    {
        ChaseX().Forget();
    }

    public async UniTaskVoid ChaseX()
    {
        while (sandBag.GetComponentInParent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            ComboText.text = $"{sandBag.transform.position.x + 4.726f:F2}M";
            await UniTask.Yield();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("BestScore", BestScore.ToString());
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
        TitleText.gameObject.SetActive(false);
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
        SetAnimation().Forget();
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "2";

        TimerBar.GetComponent<RectTransform>().DOAnchorPosY(-50f, 1f).SetEase(Ease.OutBack);
        TimerBase.GetComponent<RectTransform>().DOAnchorPosY(-50f, 1f).SetEase(Ease.OutBack);
        TimerText.GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack);
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
        ComboText.text = "";
        StarCatchGo();
    }

    public async UniTaskVoid SetAnimation()
    {
        sandBag.GetComponent<Animator>().SetTrigger("start");
        float angle = -90f;
        while(angle > -150f)
        {
            angle = Mathf.Lerp(angle, -150f, Time.deltaTime * 2f);
            sandBag.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            await UniTask.Yield();
        }
    }
    public void StarCatchGo()
    {
        StarForceTimer().Forget();
    }

    public async UniTaskVoid StarForceTimer()
    {
        StarCatchUI.gameObject.SetActive(true);
        Timer = 6f;

        while (Timer > 3)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 6f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            await UniTask.Yield();
        }

        var main = StarCatchTimerParticleSystem.main;
        StarCatchTimerParticle.SetActive(true);
        main.simulationSpeed = 1f / Time.timeScale;
        while (Timer > 0)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 3f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            StarCatchTimerParticle.GetComponent<RectTransform>().anchoredPosition = new Vector2(TimerBar.GetComponent<RectTransform>().rect.width * TimerBar.fillAmount, 0);
            await UniTask.Yield();
        }
        main.loop = false;
        CameraZoomFeedback.SmoothZoom(55, 0.1f).Forget();

        await UniTask.WaitForSeconds(1f, true);
        TimerBase.gameObject.SetActive(false);

        TimerBar.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(false);

    }

    public void ShowCriticalText()
    {
        Count++;
        Instantiate(CriticalText, CriticalUI.transform);
    }
}
