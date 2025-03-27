using UnityEngine;

public class SandBag : MonoBehaviour
{
    public Rigidbody2D Rb;
    public Vector2[] directions;
    public Rigidbody2D[] obstacles;
    private void Start()
    {
        StarCatchBarUI.Instance.OnStarCatchCompleted += FlyAway;
    }


    private void FlyAway()
    {
        foreach(var obs in obstacles)
        {
            obs.constraints = RigidbodyConstraints2D.None;
        }
        Vector2 direction = directions[(int)StarCatchBarUI.Instance.Howmuch].normalized;
        if (StarCatchBarUI.Instance.Howmuch == SuccessRate.Bad)
        {
            if (Random.Range(0, 101) == 1)
            {
                direction = directions[5];
            }
        }
        Rb.simulated = true;
        Rb.AddForce(direction * UI_Game.Instance.Count * StarCatchBarUI.Instance.multipler, ForceMode2D.Impulse);
        Rb.AddTorque(UI_Game.Instance.Count * 10);
    }


}
