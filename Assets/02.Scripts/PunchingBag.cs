using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    public event Action<float> OnPunchingBagMoveEnd;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    public Vector2[] directions;
    public float[] multiplers;

    public GameObject HitParticle;
    public ParticleSystem HitParticleSystem;
    public GameObject CrashParticlePrefab;
    public GameObject FlyParticle;
    public ParticleSystem FlyParticleSystem;
    public AudioClip LandSound;
    public AudioClip CrushSound;
    public AudioSource source;
    public Transform pTransform;
    public CameraZoomFeedback cam;

    private float _time;
    private bool _onPunchingBagMoveEnd;
    private void Start()
    {
        StarCatchUI.Instance.OnStarCatchCompleted += FlyAway;
    }

    Vector3 lastPos;
    float _landTime;
    private void FixedUpdate()
    {
        if (_rigidbody2D.simulated == false) return;
        if(lastPos == transform.position || _rigidbody2D.linearVelocity.magnitude < 0.05f)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _time = 0;
        }
        if(pTransform.position.y <= -3)
        {
            _landTime += Time.deltaTime;
        }
        else
        {
            _landTime = 0;
        }

        if (_landTime > 1)
        {
            if (!p)
            {
                p = true;
                SetAnimation();
            }
        }
        if (_time > 0.6 && !_onPunchingBagMoveEnd)
        {
            _onPunchingBagMoveEnd = true;

            SetAnimation();
            GetComponentInParent<Transform>().rotation = Quaternion.Euler(0f, 0f, 90f);
            // ¿Ãµø ≥°
            OnPunchingBagMoveEnd?.Invoke(transform.position.x + 4.726f);

            transform.localRotation = Quaternion.Euler(90f, -90, 88);
        }
        lastPos = transform.position;


        if(transform.position.y < -10f)
        {
            _rigidbody2D.linearVelocity = Vector3.zero;
            _rigidbody2D.angularVelocity = 0;
            _rigidbody2D.position = Vector3.zero;
            _rigidbody2D.linearVelocity = Vector3.left;
        }
    }

    public Animator animator;

    private void FlyAway(SuccessRate rate)
    {

        
        animator.SetTrigger("Fly");
        Vector2 direction = directions[(int)rate].normalized;
        var emission = HitParticleSystem.emission;

        var burst = new ParticleSystem.Burst[1];
        if (rate < SuccessRate.Bad)
        {
            burst[0] = new ParticleSystem.Burst(0.0f, 200, 4-(int)rate, 0.01f);
            emission.SetBursts(burst);
        }
        else
        {
            burst[0] = new ParticleSystem.Burst(0.0f, 50);
            emission.SetBursts(burst);
        }
        
        if (rate == SuccessRate.Bad)
        {
            if (UnityEngine.Random.Range(0, 101) == 1)
            {
                direction = directions[5];
                multiplers[(int)rate] = 10;
            }
        }

        

        if(rate != SuccessRate.Bad)
        {
            StartCoroutine(TimeStop());
        }
        _rigidbody2D.simulated = true;
        _rigidbody2D.AddForce(direction * UI_Game.Instance.ComboCount * multiplers[(int)rate], ForceMode2D.Impulse);
        _rigidbody2D.AddTorque(UI_Game.Instance.ComboCount * 5);
        FlyParticle.SetActive(true);
        transform.localRotation = Quaternion.Euler(90f, -90, 88);
    }

    public IEnumerator TimeStop()
    {
        Time.timeScale = 0.1f;
        HitParticle.SetActive(true);
        cam.SmoothZoomEffect(30, 0.3f).Forget();
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 1f;
    }
    public void SetAnimation()
    {
        animator.SetTrigger("Land");
        source.PlayOneShot(LandSound);
    }

    public void SpawnSmoke()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, -3.75f);
        Instantiate(CrashParticlePrefab, spawnPos, Quaternion.identity);
        if(!FlyParticleSystem.isPlaying)
            FlyParticleSystem.Play();
        var main = FlyParticleSystem.main;
        main.loop = false;

        source.PlayOneShot(CrushSound);
    }
    bool p = false;
    public void Stay()
    {
    }

    public void Exit()
    {
        _time = 0;
    }

}
