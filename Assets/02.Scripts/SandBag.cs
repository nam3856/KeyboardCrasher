using UnityEngine;

public class SandBag : MonoBehaviour
{
    public Rigidbody2D Rb;
    public Vector2[] directions;
    private void Start()
    {
        StarCatchUI.Instance.OnStarCatchCompleted += FlyAway;
    }

    public Animator animator;

    private void FlyAway()
    {
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
    }


}
