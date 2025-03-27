using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

public class SoundStageController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;    // ��ݿ� AudioSource
    [SerializeField] private AudioSource slowSfxSource; // ���ο� ȿ���� ����

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

        await UniTask.Delay(10000, DelayType.UnscaledDeltaTime);
        WaitAndStartDrum().Forget();
    }

    private async UniTaskVoid WaitAndStartDrum()
    {
        //slowSfxSource.PlayOneShot(slowSFX);

        // �巳 ���� ��� ��Ÿĳġ �ٷ� ���� (�巳�� ���ŵǾ��ų� �ڿ� ��ġ�� ���� ����)
        StarCatchBarUI.Instance.TimerEnd();

        ApplyLowPass();
        await UniTask.Delay(6000, DelayType.UnscaledDeltaTime);
        RemoveLowPass();

        StarCatchBarUI.Instance.ForceStarCatchEnd();
        PlayBGM(mainBGM, loop: true);
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
