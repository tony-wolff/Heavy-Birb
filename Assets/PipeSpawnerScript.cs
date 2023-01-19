using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public GameObject pipeCherry;
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
        if (SceneScript.isLegacy)
            Instantiate(pipe, randomHeight, transform.rotation);
        //If game mode is Heavy Birb, 1/3 of chance to have a cherry besides the pipes
        else
        {
            Instantiate(pipeCherry, randomHeight, transform.rotation);
            if(hasCherry())
                pipeCherry.transform.GetChild(2).gameObject.SetActive(true);
            else
                pipeCherry.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    bool hasCherry()
    {
        if (Random.value > (2.0/3.0) )
            return true;
        return false;
    }
}
