using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class birb_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicManagerScript logic;
    public bool birbAlive = true;
    private Camera cam;
    public SpriteRenderer mySprite;
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        m_Animator = gameObject.GetComponentInChildren<Animator>();
        //Convert sprite size to screen points
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    //Game Over condition
    bool IsOffScreen()
    {
        Vector2 top = cam.WorldToViewportPoint(mySprite.bounds.max);
        Vector2 bottom = cam.WorldToViewportPoint(mySprite.bounds.min);
        if(top.y < 0 || bottom.y > 1)
            return true;
        return false;
    }

    // Update is called once per frames
    void Update()
    {
        if (IsOffScreen())
        {
            logic.gameOver();
            birbAlive = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && birbAlive)
        {
            myRigidBody.velocity = Vector2.up * flapStrength;
            m_Animator.SetTrigger("Flapping");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birbAlive = false;
    }
}
