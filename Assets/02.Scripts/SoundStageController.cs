using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

public class SoundStageController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;    // 브금용 AudioSource
    [SerializeField] private AudioSource slowSfxSource; // 슬로우 효과음 전용

    [Header("Audio Clips")]
    [SerializeField] private AudioClip introBGM;
    [SerializeField] private AudioClip beforeStartBGM;
    [SerializeField] private AudioClip drumLoop;
    [SerializeField] private AudioClip mainBGM;
    [SerializeField] private AudioClip slowSFX;

    public static SoundStageController Instance;

    public AudioMixer audioMixer;

    [Header("Settings")]
    [SerializeField] private float introDuration = 10f;

    private bool hasStarted = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UI_Game.Instance.OnTimerEnd += TimerEnd;
        PlayBGM(beforeStartBGM, loop: true);
    }

    public void StartIntroBGM()
    {
        PlayBGM(introBGM, loop: false);
    }

    private void TimerEnd()
    {
        WaitAndStartDrum().Forget();
    }

    private async UniTaskVoid WaitAndStartDrum()
    {
        ApplyLowPass();
        await UniTask.Delay(6000, DelayType.UnscaledDeltaTime);
        RemoveLowPass();

        StarCatchUI.Instance.ForceStarCatchEnd();
    }

    public void ApplyLowPass()
    {
        audioMixer.SetFloat("LowpassFreq", 800f);
    }

    public void RemoveLowPass()
    {
        audioMixer.SetFloat("LowpassFreq", 22000f);
    }

    private void PlayBGM(AudioClip clip, bool loop)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
}
