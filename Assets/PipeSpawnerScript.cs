using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 10;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
            timer += Time.deltaTime;
        else
        {
            SpawnPipe();
            timer = 0;
        }

    }

    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestpoint = transform.position.y + heightOffset;
        Vector3 randomHeight = new Vector3(transform.position.x, Random.Range(lowestPoint, highestpoint), 0);
        Instantiate(pipe, randomHeight, transform.rotation);
    }
}
