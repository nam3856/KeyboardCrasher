using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SandBag : MonoBehaviour
{
    public Rigidbody2D Rb;
    public Vector2[] directions;
    [SerializeField]
    private float _time;

    public GameObject HitParticle;
    public GameObject CrashParticlePrefab;
    public GameObject FlyParticle;
    public ParticleSystem FlyParticleSystem;
    public GameObject UIParticle;
    private void Start()
    {
        StarCatchUI.Instance.OnStarCatchCompleted += FlyAway;
    }

    public Animator animator;

    private void FlyAway()
    {

        HitParticle.SetActive(true);
        animator.SetTrigger("Fly");
        Vector2 direction = directions[(int)StarCatchUI.Instance.Howmuch].normalized;
        if (StarCatchUI.Instance.Howmuch == SuccessRate.Bad)
        {
            if (Random.Range(0, 101) == 1)
            {
                direction = directions[5];
            }
        }
        Rb.simulated = true;
        Rb.AddForce(direction * UI_Game.Instance.Count * StarCatchUI.Instance.multipler, ForceMode2D.Impulse);
        Rb.AddTorque(UI_Game.Instance.Count * 10);
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
            if (Rb.linearVelocity == Vector2.zero)
            {
                if (!UIParticle.activeSelf)
                {
                    UIParticle.SetActive(true);

                    UI_Game.Instance.BestScore = Mathf.Max(transform.position.x + 4.726f, UI_Game.Instance.BestScore);
                    UI_Game.Instance.Save();
                }
            }
        }
    }

    public void Exit()
    {
        _time = 0;
    }

}
