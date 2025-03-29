using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class CloudSpawner : MonoBehaviour
{
    public GameObject CloudPrefab;
    public Sprite[] Sprites;
    public List<float> speed;
    public List<Transform> clouds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < 1000; i++)
        {

            float yPos = Random.Range(11f, 100f);
            float scale = Random.Range(0.8f, 1.2f);
            int index = Random.Range(0, Sprites.Length);
            speed.Add(Random.Range(0.2f, 0.8f));
            var cloud = Instantiate(CloudPrefab, new Vector3(i * 10, yPos), Quaternion.identity);
            cloud.transform.localScale = new Vector3(scale, scale, scale);
            cloud.GetComponent<SpriteRenderer>().sprite = Sprites[index];
            clouds.Add(cloud.transform);
        }
    }

    private void Update()
    {
        for (int i = 0; i < clouds.Count; i++)
        {
            clouds[i].Translate(Vector2.left * speed[i] * Time.deltaTime);
        }
    }

}
