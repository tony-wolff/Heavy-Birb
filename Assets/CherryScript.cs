using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryScript : MonoBehaviour
{
    // Script to move cherry along with the pipes
    // Allows player to pick cherry
    public LogicManagerScript logic;
    public static float speed;
    public static float oldSpeed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        oldSpeed = speed;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    private void Update()
    {
        transform.position = transform.position + (Vector3.left * speed) * Time.deltaTime;
        if (transform.position.x < -45)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            logic.addScore(1);
            Destroy(gameObject);
        }
    }
}
