using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public PlayerInput playerInput;
    public string actionName1 = "Attack";
    public string actionName2 = "Interact";
    public TMP_Text binding1DisplayText;
    public TMP_Text binding2DisplayText;
    public Button rebindButton1;
    public Button rebindButton2;

    public Slider MusicSlider;
    public Slider SfxSlider;

    private InputAction attack1Action;
    private InputAction attack2Action;

    public TextMeshProUGUI displayText;

    public AudioMixer mixer;
    public GameObject BindPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {

        if (PlayerPrefs.HasKey("input_bindings"))
        {
            string loadedJson = PlayerPrefs.GetString("input_bindings");
            playerInput.actions.LoadBindingOverridesFromJson(loadedJson);
        }
    }
    void Start()
    {

        UI_Game.Instance.KeySetting();
        attack1Action = playerInput.actions[actionName1];
        attack2Action = playerInput.actions[actionName2];


        UpdateBindingTexts();
        float bgmVolume;
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            SetMusicVolume(bgmVolume);
        }
        else
        {
            bgmVolume = Mathf.Pow(10, GetMixerVolume("BGMVolume") / 20f);
        }
        MusicSlider.value = bgmVolume;

        float sfxVolume;
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume(sfxVolume);
        }
        else
        {
            sfxVolume = Mathf.Pow(10, GetMixerVolume("SFXVolume") / 20f);
        }
        SfxSlider.value = sfxVolume;

        rebindButton1.onClick.AddListener(() =>
        {
            RebindAction(actionName1, 0);
        });

        rebindButton2.onClick.AddListener(() =>
        {
            RebindAction(actionName2, 0);
        });

        MusicSlider.onValueChanged.AddListener((v) =>
        {
            SetMusicVolume(v);
            SaveVolume();
        });
        SfxSlider.onValueChanged.AddListener((v) =>
        {
            SetSFXVolume(v);
            SaveVolume();
        });

        gameObject.SetActive(false);
    }

    float GetMixerVolume(string parameter)
    {
        float value;
        mixer.GetFloat(parameter, out value);
        return value;
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }
    void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }

    // Update is called once per frame
    void UpdateBindingTexts()
    {
        binding1DisplayText.text = attack1Action.GetBindingDisplayString(0);
        binding2DisplayText.text = attack2Action.GetBindingDisplayString(0);
    }
    public void RebindAction(string actionName, int bindingIndex)
    {
        BindPanel.SetActive(true);
        var actionToRebind = playerInput.actions[actionName];
        if (actionToRebind.enabled)
            actionToRebind.Disable();
        rebindButton1.interactable = false;
        rebindButton2.interactable = false;
        actionToRebind.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .OnComplete(operation =>
            {
                string newBindingPath = actionToRebind.bindings[bindingIndex].effectivePath;

                RemoveDuplicateBindings(actionName, newBindingPath);

                UpdateBindingTexts();
                rebindButton1.interactable = true;
                rebindButton2.interactable = true;
                UI_Game.Instance.KeySetting();

                string json = playerInput.actions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("input_bindings", json);
                PlayerPrefs.Save();

                BindPanel.SetActive(false);
                operation.Dispose();

                actionToRebind.Enable();
            })
            .Start();
    }

    void RemoveDuplicateBindings(string currentActionName, string newPath)
    {
        foreach (var action in playerInput.actions)
        {
            if (action.name == currentActionName) continue;

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].effectivePath == newPath)
                {
                    action.ApplyBindingOverride(i, new InputBinding { overridePath = null });
                    Debug.Log($"Removed duplicate binding in {action.name}");
                }
            }
        }
    }

    void SaveVolume()
    {
        PlayerPrefs.SetFloat("BGMVolume", MusicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SfxSlider.value);
    }
}
