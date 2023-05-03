using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public GameObject cherry;
    public float heightOffset;
    public float spawnRate;
    public float speedIncrease;
    public float spawnRateDecrease = 0.5f;
    float cherrySpanwRate;
    float oldSpawnRate;
    private float timer;
    private float timerCherry;
    float timeElapsed;
    float lerpDuration = 10;
    float oldSpeedX;

    LogicManagerScript logic;
    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0;
        timer = 0;
        timerCherry = 0;
        oldSpeedX = pipeMoving_script.moveSpeedX;
        oldSpawnRate = spawnRate;
        cherrySpanwRate = spawnRate * 1.5f;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
        SpawnPipe();
        //If not legacy game, deactivate points when passing between pipes
        if (!SceneScript.isLegacy)
        {
            Debug.Log(pipe.transform.GetChild(2).gameObject);
            pipe.transform.GetChild(2).gameObject.SetActive(false);

        }
    }

    public void Reset()
    {
        spawnRate = 3;
        lerpDuration = 10;
        pipeMoving_script.moveSpeedX = 5;
    }

    // Update is called once per frame
    void Update()
    {
        //Increase speed when player score hits a threshold;
        if (logic.IncreaseDifficultyOn() && !logic.IsGameOver() && !SceneScript.isLegacy)
        {
            //Max difficulty
            if (logic.getScore() >= 40)
            {
                if(timeElapsed < lerpDuration)
                {
                    spawnRate = Mathf.Lerp(oldSpawnRate, 1, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    spawnRate = 1.5f;
                    logic.setDifficulty(false);
                }

            }
            else
            {
                //:BUG: Spawn rate increases too much after a certain time
                //:BUG: updates cherry speed
                //:TODO: adjust difficulty after 30 points, custom shader in mario invicibility style
                //:TODO: Delete pipeCherry, make spawner cherry
                //:BEHAVIOR: Speed the pipes linearly on a 60 second scale until it reaches oldspeed + speedIncrease;
                if (timeElapsed < lerpDuration)
                {
                    pipeMoving_script.moveSpeedX = Mathf.Lerp(oldSpeedX, oldSpeedX + speedIncrease, timeElapsed / lerpDuration);
                    spawnRate = Mathf.Lerp(oldSpawnRate, oldSpawnRate - spawnRateDecrease, timeElapsed / lerpDuration);
                    pipeMoving_script.moveSpeedY = pipeMoving_script.moveSpeedX;
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    //Set to their target value after a certain duration
                    pipeMoving_script.moveSpeedX = oldSpeedX + speedIncrease;
                    pipeMoving_script.moveSpeedY = oldSpeedX + speedIncrease;
                    spawnRate = oldSpawnRate - spawnRateDecrease;
                    //Update old value
                    oldSpeedX += speedIncrease;
                    oldSpawnRate -= spawnRateDecrease;
                    logic.setDifficulty(false);
                }
            }
        }
        else
        {
            timeElapsed = 0;
        }


        if (timer < spawnRate)
            timer += Time.deltaTime;
        else
        {
            SpawnPipe();
            timer = 0;
        }

        if (!SceneScript.isLegacy)
        {
            if (timerCherry < cherrySpanwRate)
                timerCherry += Time.deltaTime;
            else
            {
                SpawnCherry();
                timerCherry = 0;
            }
        }
    }

    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestpoint = transform.position.y + heightOffset;
        Vector3 randomHeight = new Vector3(transform.position.x, Random.Range(lowestPoint, highestpoint), 0);
        Instantiate(pipe, randomHeight, transform.rotation);
    }

    void SpawnCherry()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestpoint = transform.position.y + heightOffset;
        Vector3 randomHeight = new Vector3(transform.position.x, Random.Range(lowestPoint, highestpoint), 0);
        Instantiate(cherry, randomHeight, transform.rotation);
    }
}
