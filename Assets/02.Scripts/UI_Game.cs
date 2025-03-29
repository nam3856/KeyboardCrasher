using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public static UI_Game Instance;
    private Tween _tween;
    public PlayerInput playerInput;
    public string Attack1 = "Attack";
    public string Attack2 = "Interact";
    public AudioSource source;

    public string Key1;
    public string Key2;
    [Header("Title")]
    public TextMeshProUGUI TitleText;
    public Button StartButton;
    public Button RandomButton;
    public Button RankingButton;
    public TMP_InputField NicknameInput;
    public string Name;
    public TextMeshProUGUI RandomText;
    public GameObject MainPanel;
    public TextMeshProUGUI NicknameText;

    [Header("Timer")]
    public float Timer = 10;
    public Image TimerBar;
    public Image TimerBase;
    public TextMeshProUGUI TimerText;
    public AudioClip TimerStartSound;

    [Header("Start Timer")]
    public TextMeshProUGUI StartTimeText;
    public TextMeshProUGUI StartInstructionText;
    public TextMeshProUGUI StartTimerText;

    [Header("Combo")]
    public TextMeshProUGUI ComboText;
    public int ComboCount;

    [Header("Critical")]
    public GameObject CriticalUI;
    public GameObject CriticalText;
    
    [Header("StarCatch")]
    public Canvas StarCatchUI;
    public GameObject StarCatchTimerParticle;
    public ParticleSystem StarCatchTimerParticleSystem;
    public string[] SuccessTexts;
    [SerializeField]private TextMeshProUGUI _successText;
    public Color32[] SuccessColors;

    [Header("Score")]
    public float BestScore = 0;
    public TextMeshProUGUI NewRecordText;
    public GameObject BestRecordWindow;
    public TextMeshProUGUI BestScoreText;
    public GameObject ScoreParticle;
    private Sequence[] _successTextTweenSequence;
    private Vector3 _bestScoreWindowPos;
    public GameObject BestRecordPrefab;
    public AudioClip UISound;
    public AudioClip NewRecord;
    public AudioClip TimerSound;
    
    public CameraZoomFeedback CameraZoomFeedback;
    public GameObject PunchingBagObject;
    public PunchingBag PunchingBagScript;

    public event Action OnTimerEnd;
    public event Action OnTimerStart;
    public event Action<float> OnBestScoreChanged;

    [Header("Ranking")]
    public GameObject RankingPanel;
    public GameObject RankingEntryTemplate;
    public GameObject RankingUI;
    public Button ExitButton;

    public GameObject Key1Obj;
    public GameObject Key2Obj;

    [Header("Setting")]
    public GameObject SettingUI;
    public Button SettingButton;
    public Button SettingExitButton;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        StartButton.onClick.AddListener(StartTimer);

        RandomButton.onClick.AddListener(SetRandomNickname);
        RankingButton.onClick.AddListener(ShowRankingButtonClicked);
        ExitButton.onClick.AddListener(HideRanking);

        SettingButton.onClick.AddListener(OpenSetting);
        SettingExitButton.onClick.AddListener(CloseSetting);
        SequenceInit();
    }
    private void ShowRankingButtonClicked()
    {
        MainPanel.SetActive(false);
        ShowRanking();
    }


    public void ButtonClickSound()
    {
        source.PlayOneShot(UISound);
    }
    private void SetRandomNickname()
    {
        string nickname = NicknameGenerator.GenerateKoreanNickname();
        NicknameInput.text = nickname;
    }
    private void SequenceInit()
    {
        _successTextTweenSequence = new Sequence[5];

        for (int i = 0; i < _successTextTweenSequence.Length; i++)
        {
            _successTextTweenSequence[i] = DOTween.Sequence().Pause();
        }
        _successTextTweenSequence[0].Append(_successText.transform.DOScale(3f, 0.01f));
        _successTextTweenSequence[0].Append(_successText.transform.DOScale(1f, 0.2f).SetEase(Ease.InElastic)).Join(_successText.DOColor(SuccessColors[0], 0.2f));
        _successTextTweenSequence[1].Append(_successText.transform.DOScale(2f, 0.01f));
        _successTextTweenSequence[1].Append(_successText.transform.DOScale(1f, 0.2f).SetEase(Ease.InElastic)).Join(_successText.DOColor(SuccessColors[1], 0.2f));
        _successTextTweenSequence[2].Append(_successText.transform.DOScale(1.5f, 0.01f));
        _successTextTweenSequence[2].Append(_successText.transform.DOScale(1f, 0.2f).SetEase(Ease.InElastic)).Join(_successText.DOColor(SuccessColors[2], 0.2f));
        _successTextTweenSequence[3].Append(_successText.DOColor(Color.clear, 0.01f));
        _successTextTweenSequence[3].Append(_successText.DOColor(SuccessColors[3], 0.2f));
        _successTextTweenSequence[4].Append(_successText.DOColor(Color.clear, 0.01f));
        _successTextTweenSequence[4].Append(_successText.DOColor(SuccessColors[4], 0.2f)).Join(_successText.transform.DOScale(0.6f, 0.2f));

        for (int i = 0; i < _successTextTweenSequence.Length; i++)
        {
            _successTextTweenSequence[i].Append(_successText.DOColor(Color.clear, 4f).SetEase(Ease.InOutExpo).OnComplete(() => {
                _successText.transform.localScale = new Vector3(1, 1, 1);
                _successText.color = Color.white;
                _successText.gameObject.SetActive(false);
            }));
        }
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

                BestScoreText.text = $"{0:F3}M";
            }
        }
        else
        {
            Debug.Log("로드 실패 - string is null or empty");

            BestScoreText.text = $"{0:F3}M";
        }
        string name = PlayerPrefs.GetString("Name");
        if(!string.IsNullOrEmpty(name))
        {
            Name = name;
            RandomButton.gameObject.SetActive(false);
            NicknameInput.gameObject.SetActive(false);
            NicknameText.text = $"다시 오셨군요, {name} 님!";
            NicknameText.gameObject.SetActive(true);
            RandomText.gameObject.SetActive(false);
        }
        global::StarCatchUI.Instance.OnStarCatchCompleted += ShowSuccessRateText;
        TitleText.transform.DOScale(0.8f, 0.5f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
        PunchingBagScript.OnPunchingBagMoveEnd += UpdateBestScoreAndShowNewRecordText;
        KeySetting();
        _bestScoreWindowPos = BestRecordWindow.GetComponent<RectTransform>().anchoredPosition;
    }

    void OpenSetting()
    {
        SettingUI.SetActive(true);
        MainPanel.SetActive(false);
    }

    void CloseSetting()
    {
        SettingUI.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void KeySetting()
    {

        InputAction attack1 = playerInput.actions[Attack1];
        InputAction attack2 = playerInput.actions[Attack2];

        Key1 = attack1.bindings[0].ToDisplayString();
        Key2 = attack2.bindings[0].ToDisplayString();
    }
    public void ShowSuccessRateText(SuccessRate rate)
    {
        _successText.text = SuccessTexts[(int)rate];

        _successText.gameObject.SetActive(true);
        _successTextTweenSequence[(int)rate].Play();
        ChaseX().Forget();
    }

    public void ShowRanking()
    {
        RankingManager.Instance.GetTopRankings(10, DisplayRanking);
    }

    public void DisplayRanking(RankingManager.RankingList list)
    {
        RankingUI.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            GameObject newEntry = Instantiate(RankingEntryTemplate, RankingPanel.transform);
            newEntry.SetActive(true);

            var texts = newEntry.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = $"#{i + 1}";
            if (i < list.rankings.Count)
            {
                texts[1].text = list.rankings[i].nickname;
                texts[2].text = $"{list.rankings[i].bestScore:F2}M";
                texts[3].text = $"{list.rankings[i].bestCombo} Combo";
            }
        }
    }
    public async UniTaskVoid ShowKeys()
    {
        Key1Obj.GetComponent<KeyVisual>().keyBind = Key1;
        Key2Obj.GetComponent<KeyVisual>().keyBind = Key2;

        Key1Obj.SetActive(true);
        await UniTask.Delay(50);

        Key2Obj.SetActive(true);
    }
    public void HideRanking()
    {
        foreach (Transform child in RankingPanel.transform)
        {
            Destroy(child.gameObject);
        }
        RankingUI.SetActive(false);

        if(PunchingBagObject.transform.position.x != -4.726f)
        {
            FadeManager.Instance.RestartSceneWithFade();
        }
        else
        {
            MainPanel.SetActive(true);
        }
    }
    public async UniTaskVoid ChaseX()
    {
        Instantiate(BestRecordPrefab,new Vector3(BestScore-4.726f,0,0), Quaternion.identity);
        BestRecordWindow.GetComponent<RectTransform>().DOAnchorPos(_bestScoreWindowPos, 2).SetEase(Ease.OutExpo);
        while (PunchingBagObject.GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            ComboText.text = $"{PunchingBagScript.gameObject.transform.position.x + 4.726f:F2}M";
            await UniTask.Yield();
        }
    }

    public void UpdateBestScoreAndShowNewRecordText(float score)
    {
        if (BestScore < score)
        {
            float prev = BestScore;
            BestScore = score;
            UpdateBestScore(prev, score).Forget();
            ScoreParticle.SetActive(true);
        }
        else
        {
            ShowRanking();
        }
    }

    public async UniTaskVoid UpdateBestScore(float prevScore, float newScore)
    {
        OnBestScoreChanged?.Invoke(BestScore);

        float increase = 0.001f;
        while (prevScore < (int)newScore)
        {
            prevScore += increase;
            BestScoreText.text = $"{prevScore:F3}M";
            await UniTask.Yield();
            increase *= 1.5f;
        }
        BestScoreText.text = $"{newScore:F3}M";
        await UniTask.Delay(200);

        NewRecordText.gameObject.SetActive(true);
        NewRecordText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic);
        source.PlayOneShot(NewRecord);
        await UniTask.Delay(500);

        ShowRanking();
        
    }


    public void Add()
    {
        ComboCount++;
        float timeRatio = Timer / 10f;
        float threshold = Mathf.Lerp(100f, 5f, timeRatio);
        ComboText.fontSize = Mathf.Clamp((float)ComboCount / 2, 40f, 120f);
        ComboText.text = ComboCount >= threshold ? $"Combo x {ComboCount}!" : $"Combo x {ComboCount}";

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
        MainPanel.SetActive(false);
        if(string.IsNullOrEmpty(Name)) 
        {
            if (string.IsNullOrEmpty(NicknameInput.text))
            {
                Name = NicknameGenerator.GenerateKoreanNickname();
            }
            else
            {
                if (string.IsNullOrEmpty(NicknameInput.text.Trim()))
                {
                    Name = NicknameGenerator.GenerateKoreanNickname();
                }
                else
                {
                    Name = NicknameInput.text.Trim();
                }
            }
        }
        PlayerPrefs.SetString("Name", Name);
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
        StartInstructionText.text = $"Press {Key1}, {Key2} Key To Attack!";
        SetAnimation().Forget();
        source.PlayOneShot(TimerSound);
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "2";

        source.PlayOneShot(TimerSound);
        TimerBar.GetComponent<RectTransform>().DOAnchorPosY(-50f, 1f).SetEase(Ease.OutBack);
        TimerBase.GetComponent<RectTransform>().DOAnchorPosY(-50f, 1f).SetEase(Ease.OutBack);
        TimerText.GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack);
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "1";

        source.PlayOneShot(TimerSound);
        await UniTask.WaitForSeconds(1);
        StartTimeText.text = "START!";

        source.PlayOneShot(TimerStartSound);
        OnTimerStart?.Invoke();
        ShowKeys().Forget();
        StartTimeText.transform.DOScale(2f, 0.01f).OnComplete(() => StartTimeText.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            StartTimeText.gameObject.SetActive(false);
        }));

        StartTimerText.gameObject.SetActive(false);
        BestRecordWindow.GetComponent<RectTransform>().DOAnchorPosX(996, 2).SetEase(Ease.InExpo);
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

        StarForceTimer().Forget();
    }

    public async UniTaskVoid SetAnimation()
    {
        PunchingBagScript.gameObject.GetComponent<Animator>().SetTrigger("start");
        float angle = -90f;
        while(angle > -149f)
        {
            angle = Mathf.Lerp(angle, -150f, Time.deltaTime * 4f);
            PunchingBagScript.gameObject.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            await UniTask.Yield();
        }
    }

    public async UniTaskVoid StarForceTimer()
    {
        Key1Obj.SetActive(false);
        Key2Obj.SetActive(false);
        StarCatchUI.gameObject.SetActive(true);
        Timer = 3f;

        for(int i = 0; i < 3; i++)
        {
            source.PlayOneShot(TimerSound);
            await UniTask.WaitForSeconds(1, true);
        }

        var main = StarCatchTimerParticleSystem.main;
        main.simulationSpeed = 1f / Time.timeScale;
        StarCatchTimerParticle.SetActive(true);

        source.PlayOneShot(TimerStartSound);
        while (Timer > 2)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 3f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            StarCatchTimerParticle.GetComponent<RectTransform>().anchoredPosition = new Vector2(TimerBar.GetComponent<RectTransform>().rect.width * TimerBar.fillAmount, 0);
            await UniTask.Yield();
        }

        source.PlayOneShot(TimerStartSound);

        while (Timer > 1)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 3f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            StarCatchTimerParticle.GetComponent<RectTransform>().anchoredPosition = new Vector2(TimerBar.GetComponent<RectTransform>().rect.width * TimerBar.fillAmount, 0);
            await UniTask.Yield();
        }

        source.PlayOneShot(TimerStartSound);

        while (Timer > 0)
        {
            Timer = Mathf.Clamp(Timer - Time.unscaledDeltaTime, 0, 3f);
            TimerBar.fillAmount = Mathf.Clamp(Timer / 3f, 0, 1f);
            StarCatchTimerParticle.GetComponent<RectTransform>().anchoredPosition = new Vector2(TimerBar.GetComponent<RectTransform>().rect.width * TimerBar.fillAmount, 0);
            await UniTask.Yield();
        }

        main.loop = false;
        CameraZoomFeedback.SmoothZoom(55, 0.1f).Forget();
        TimerBase.gameObject.SetActive(false);
        TimerBar.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(false);

    }

    public void ShowCriticalText()
    {
        ComboCount++;
        Instantiate(CriticalText, CriticalUI.transform);
    }
}
