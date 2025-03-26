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
        // ���� ��� �ȳ� ���� ����
        string instructions = "���� ���:\n1. 10�ʵ��� Ű���带 ���� ��������.\n2. 10�� �� ������ ��ġ�� �� �� �ֽ��ϴ�.\n3. ����ƺ� �ִ��� �ָ� ����������.";
        InstructionText.text = instructions;
        // �ȳ� �г� Ȱ��ȭ
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
