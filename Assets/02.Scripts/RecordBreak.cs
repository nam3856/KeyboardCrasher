using UnityEngine;

public class RecordBreak : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BreakParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var particle in BreakParticles)
        {
            Instantiate(particle, collision.transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
