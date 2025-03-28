using UnityEngine;

public class KKK : MonoBehaviour
{

    public GameObject k;
    public GameObject a;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float start = -4.726f;

        for(int i = 0; i < 150; i++)
        {
            Vector3 vector3 = new Vector3(start + i * 10, -3.79f);
            Instantiate(k, vector3, Quaternion.identity);
        }

        for (int i = 0; i < 1500; i++)
        {
            if (i % 10 == 0) continue;
            Vector3 vector3 = new Vector3(start + i, -3.79f);
            Instantiate(a, vector3, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
