using DG.Tweening;
using System.Collections;
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

    public GameObject InstructionPanel;
    public TextMeshProUGUI InstructionText;

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

        StartButton.onClick.AddListener(ShowGameInstructions);
        InstructionPanel.SetActive(false);
    }

    void ShowGameInstructions()
    {
        // 게임 방법 안내 내용 설정
        string instructions = "게임 방법:\n1. 10초동안 키보드를 마구 누르세요.\n2. 10초 뒤 마무리 펀치를 할 수 있습니다.\n3. 허수아비를 최대한 멀리 날려보세요.";
        InstructionText.text = instructions;
        // 안내 패널 활성화
        InstructionPanel.SetActive(true);
    }
    public void Add()
    {
        Count++;
        ComboText.text = Count.ToString();
        if(_tween!=null && _tween.active == true)
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
        StartCoroutine(TimerCoroutine());
    }

    public IEnumerator TimerCoroutine()
    {
        while (Timer > 0)
        {
            Timer = Mathf.Clamp(Timer - Time.deltaTime, 0, 10);
            TimerBar.fillAmount = Timer / 10;
            yield return null;
        }
    }


    public void ShowCriticalText()
    {
        Count++;
        Instantiate(CriticalText, CriticalUI.transform);
    }
}
