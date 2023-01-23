using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeMoving_script : MonoBehaviour
{
    public float moveSpeedX = 5;
    public float moveSpeedY = 5;
    public float deadZone = -45;
    float heightOffset = 10;
    bool up;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        if (Random.value > 0.5)
            up = true;
        else up = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeedX) * Time.deltaTime;
        MovePipe();
        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe deleted");
            Destroy(gameObject);
        }
    }

    void MovePipe()
    {
        if (cam.WorldToViewportPoint(transform.position).y <= 0)
            up = true;
        if (cam.WorldToViewportPoint(transform.position).y >=1)
            up = false;
        if (up)
            transform.Translate(transform.up * Time.deltaTime * moveSpeedY);
        else
            transform.Translate(-transform.up * Time.deltaTime * moveSpeedY);
    }
}
