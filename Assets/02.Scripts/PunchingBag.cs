using Cysharp.Threading.Tasks;
using System;
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



    private float _time;
    private bool _onPunchingBagMoveEnd;
    private void Start()
    {
        StarCatchUI.Instance.OnStarCatchCompleted += FlyAway;
    }

    public Animator animator;

    private void FlyAway(SuccessRate rate)
    {

        HitParticle.SetActive(true);
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
            }
        }
        _rigidbody2D.simulated = true;
        _rigidbody2D.AddForce(direction * UI_Game.Instance.ComboCount * multiplers[(int)rate], ForceMode2D.Impulse);
        _rigidbody2D.AddTorque(UI_Game.Instance.ComboCount * 10);
        FlyParticle.SetActive(true);
    }

    public async UniTaskVoid SetAnimation()
    {
        if (p) return;
        p = true;
        animator.SetTrigger("Land");
        float angleZ = 0f;
        float angleY = -150f;
        while (angleY < -90f)
        {
            angleY = Mathf.Lerp(angleY, -90f, Time.deltaTime * 4f);
            angleZ = Mathf.Lerp(angleZ, 88f, Time.deltaTime * 4f);
            transform.localRotation = Quaternion.Euler(90f, angleY, angleZ);
            await UniTask.Yield();
        }
        transform.localRotation = Quaternion.Euler(90f, -90f, 88f);
    }

    public void SpawnSmoke()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, -3.75f);
        Instantiate(CrashParticlePrefab, spawnPos, Quaternion.identity);
        var main = FlyParticleSystem.main;
        main.loop = false;
    }
    bool p = false;
    public void Stay()
    {
        _time += Time.deltaTime;
        if (_time > 0.6f)
        {
            SetAnimation().Forget();
            if (_rigidbody2D.linearVelocity == Vector2.zero)
            {
                if (!_onPunchingBagMoveEnd)
                {
                    _onPunchingBagMoveEnd = true;
                    // ¿Ãµø ≥°
                    OnPunchingBagMoveEnd?.Invoke(transform.position.x+ 4.726f);
                }
            }
        }
    }

    public void Exit()
    {
        _time = 0;
    }

}
