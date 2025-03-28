using UnityEngine;

public class PunchingBagParent : MonoBehaviour
{
    public PunchingBag child;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        child.SpawnSmoke();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        child.Stay();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        child.Exit();
    }
}
