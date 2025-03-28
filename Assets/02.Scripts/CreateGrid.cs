using UnityEngine;

public class CreateGrid : MonoBehaviour
{

    public GameObject Grid10mPrefab;
    public GameObject Grid1mPrefab;
    public float start = -4.726f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < 150; i++)
        {
            Vector3 vector3 = new Vector3(start + i * 10, -3.79f);
            Instantiate(Grid10mPrefab, vector3, Quaternion.identity);
        }

        for (int i = 0; i < 1500; i++)
        {
            if (i % 10 == 0) continue;
            Vector3 vector3 = new Vector3(start + i, -3.79f);
            Instantiate(Grid1mPrefab, vector3, Quaternion.identity);
        }
    }
}
