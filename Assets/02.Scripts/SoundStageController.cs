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
    public AudioMixer audioMixer;
    [Header("Settings")]
    [SerializeField] private float introDuration = 10f;

    private bool hasStarted = false;

    private void Start()
    {
        UI_Game.Instance.OnTimerStart += TimerStart;

        PlayBGM(beforeStartBGM, loop: true);

    }

    private void TimerStart()
    {
        PlayBGM(introBGM, loop: false);
        StartSequence().Forget();
    }
    private async UniTaskVoid StartSequence()
    {
        if (hasStarted) return;
        hasStarted = true;

        // 10초 대기 (인트로 재생)
        await UniTask.Delay(10000);

        WaitAndStartDrum().Forget();
    }

    private async UniTaskVoid WaitAndStartDrum()
    {
        //slowSfxSource.PlayOneShot(slowSFX);

        //PlayBGM(drumLoop, loop: true);
        StarCatchBarUI.Instance.TimerEnd();
        ApplyLowPass();
        await UniTask.Delay(6000);
        RemoveLowPass();
        StarCatchBarUI.Instance.ForceStarCatchEnd();

        PlayBGM(mainBGM, loop: true);
    }

    public void ApplyLowPass()
    {
        audioMixer.SetFloat("LowpassFreq", 800f); // 예: 800Hz로 제한 (저음만 통과)
    }
    public void RemoveLowPass()
    {
        audioMixer.SetFloat("LowpassFreq", 22000f); // 정상 주파수로 복구 (원음으로)
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
