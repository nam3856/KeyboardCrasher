using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public static UI_Game Instance;
    public Tween tween;
    public float timer = 10;
    public Image timerBar;
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
    public TextMeshProUGUI text;
    public int count;
    public GameObject ui;

    public GameObject CriticalText;
    public void Add()
    {
        count++;
        text.text = count.ToString();
        if(tween!=null && tween.active == true)
        {
            tween.Kill();
        }
        tween = text.transform.DOScale(3f, 0.1f)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                text.transform.DOScale(1f, 0.1f);
            });
    }

    private void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime,0,10);
        timerBar.fillAmount = timer / 10;

    }

    public void Critical()
    {
        Instantiate(CriticalText, ui.transform);
    }
}
