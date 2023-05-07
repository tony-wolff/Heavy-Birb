using System.Collections;
using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public GameObject cherry;
    public float heightOffset;
    public float spawnRate;
    public float speedIncrease;
    public float CherrySpeed;
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
        CherryScript.speed = CherrySpeed;
        CherryScript.oldSpeed = CherrySpeed;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
        //If not legacy game, deactivate points when passing between pipes
        if (!SceneScript.isLegacy)
            pipe.transform.GetChild(2).gameObject.SetActive(false);
        else
            pipe.transform.GetChild(2).gameObject.SetActive(true);
        SpawnPipe();

    }

    public void Reset()
    {
        spawnRate = 3;
        lerpDuration = 10;
        pipeMoving_script.Reset();
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
                    spawnRate = Mathf.Lerp(oldSpawnRate, 5/11f, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    spawnRate = 5/11f;
                    logic.setDifficulty(false);
                }

            }
            else
            {
                SpeedUp();
            }
        }
        if (timer < spawnRate)
            timer += Time.deltaTime;
        else
        {
            SpawnPipe();
            timer = 0;
        }

        if (!SceneScript.isLegacy && logic.getScore() < 40)
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

    private void SpeedUp()
    {
        
        float newSpawnRate = oldSpawnRate*0.8f;
        float cherrySpeedIncrease = ValueToAdd(CherryScript.oldSpeed);
        if(timeElapsed < lerpDuration)
        {
            float percentage = timeElapsed / lerpDuration;
            pipeMoving_script.moveSpeedX = Mathf.Lerp(oldSpeedX, oldSpeedX + speedIncrease, percentage);
            spawnRate = Mathf.Lerp(oldSpawnRate, newSpawnRate, percentage);
            CherryScript.speed = Mathf.Lerp(CherryScript.oldSpeed, CherryScript.oldSpeed + cherrySpeedIncrease, percentage);
            pipeMoving_script.moveSpeedY = pipeMoving_script.moveSpeedX;
            timeElapsed += Time.deltaTime;
        }
        else
        {
            //Set to their target value after a certain duration
            pipeMoving_script.moveSpeedX = oldSpeedX + speedIncrease;
            pipeMoving_script.moveSpeedY = oldSpeedX + speedIncrease;
            CherryScript.speed = CherryScript.oldSpeed + cherrySpeedIncrease;
            //Update old value
            oldSpeedX += speedIncrease;
            oldSpawnRate = newSpawnRate;
            CherryScript.oldSpeed += cherrySpeedIncrease;
            cherrySpanwRate = spawnRate * 1.5f;
            logic.setDifficulty(false);
            timeElapsed=0;
        }
    }

    float ValueToAdd(float speedToUpdate)
    {
        //The ratio between new & old speed = %
        float newSpeedRatio = (speedIncrease / oldSpeedX);

        return newSpeedRatio*speedToUpdate; 
    }
}
