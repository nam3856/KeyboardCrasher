using Cysharp.Threading.Tasks;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class PlayerMove : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public CameraZoomFeedback CameraZoomFeedback;
    public CameraShake CameraShake;
    public Rigidbody2D Rb;
    public Animator An;
    public Animator [] EffectAn;
    private float x;
    public float MoveSpeed = 5f;
    public int count;
    float lastInputTime;
    float crit;
    public AudioSource StartAudioSource;
    public AudioSource HitAudioSource;
    public AudioClip[] attackStartSounds;
    public AudioClip[] attackHitSounds;
    public Animator PunchBagAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void Start()
    {
        UI_Game.Instance.OnTimerEnd += TimerEnd;
        UI_Game.Instance.OnTimerStart += StartGame;
    }

    private void OnDisable()
    {
        UI_Game.Instance.OnTimerEnd -= TimerEnd;
        UI_Game.Instance.OnTimerStart -= StartGame;
    }
    private void Update()
    {
        float inputInterval = Time.time - lastInputTime;
        if (inputInterval > 0.6f)
        {
            An.speed = 1f;
        }
    }
    void StartGame()
    {
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    void TimerEnd()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Disable();
        UniTask.RunOnThreadPool(async () =>
        {
            await UniTask.Delay(50);
        });
        CameraZoomFeedback.SmoothZoom(30, 2f).Forget();
        An.SetTrigger("Charge");
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        int rand = Random.Range(0, 6);
        
        An.SetInteger("rand", rand);
        An.SetTrigger("Attack");
        rand = Random.Range(0, 2);
        float inputInterval = Time.time - lastInputTime;
        float normalizedSpeed = Mathf.Clamp(0.5f / inputInterval, 1f, 5f);
        Debug.Log(normalizedSpeed);
        lastInputTime = Time.time;
        An.speed = normalizedSpeed;

        crit = Random.Range(30f, 50f);
        CameraShake.Shake();

        if (crit <= 35f)
        {

            CameraZoomFeedback.SmoothZoomEffect(crit, 0.01f).Forget();
            UI_Game.Instance.ShowCriticalText();
            PunchBagAnimator.SetBool("Critical", true);
        }
        else
        {
            PunchBagAnimator.SetBool("Critical", false);
        }
        PunchBagAnimator.SetTrigger("Hit");
        UI_Game.Instance.Add();

    }
    public void OnAnimationCompleted(int rand)
    {
        EffectAn[count].SetInteger("rand", rand);
        EffectAn[count].SetTrigger("Attack");
        count = (count + 1) % (EffectAn.Length - 1);
    }

    public void PlayAttackSound(int idx)
    {
        //int audioIndex;
        //if (idx >= 0 && idx < 3)
        //{
        //    audioIndex = Random.Range(0, 5);
        //}
        //else
        //{
        //    audioIndex = Random.Range(5, 10);
        //}
        //var clip = attackStartSounds[audioIndex];
        //StartAudioSource.PlayOneShot(clip);
    }

    int soundIdx = 0;
    public void PlayAttackHitSound()
    {
        var clip = attackHitSounds[soundIdx];
        HitAudioSource.PlayOneShot(clip);

        soundIdx = (soundIdx + 1) % attackHitSounds.Length;
        HitAudioSource.pitch = Random.Range(0.95f, 1.05f);
    }

    public void StartCameraZoom(float val)
    {
        Debug.Log("StartCameraZoom");
    }

}
