using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 시

public class KeyVisual : MonoBehaviour
{
    public string keyBind = "A"; // 표시할 키
    public TMP_Text keyText; // 또는 UnityEngine.UI.Text
    public Vector3 pressedOffset = new Vector3(0, -10, 0);
    public Vector3 depressedOffset = new Vector3(0, -7, 0);
    public Color normalColor;
    public Color pressedColor;
    public Color depressedColor;
    public Sprite[] sprites;
    public Image Image;

    private Vector3 originalPosition;

    public int KeyIndex;

    void Start()
    {
        keyText.text = keyBind;
        originalPosition = keyText.rectTransform.anchoredPosition;
    }
    float _t;
    bool pressed = false;
    void Update()
    {
        _t += Time.deltaTime;

        if (_t>0.1f && !pressed)
        {
            pressed = true;
            keyText.rectTransform.anchoredPosition = originalPosition + pressedOffset;
            keyText.color = pressedColor;
            Image.sprite = sprites[1];
        }

        if(_t>0.2f && pressed)
        {
            keyText.rectTransform.anchoredPosition = originalPosition + depressedOffset;
            keyText.color = depressedColor;

            Image.sprite = sprites[2];
        }

        if(_t>0.3f && pressed)
        {
            _t = 0;
            pressed = false;
            keyText.rectTransform.anchoredPosition = originalPosition;
            keyText.color = normalColor;
            Image.sprite = sprites[0];
        }
    }
}
