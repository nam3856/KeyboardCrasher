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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, -3.75f);
        Instantiate(CrashParticlePrefab, spawnPos, Quaternion.identity);
        var main = FlyParticleSystem.main;
        main.loop = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _time += Time.deltaTime;

        if (_time > 0.6f)
        {
            animator.SetTrigger("Land");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _time = 0;
    }

}
