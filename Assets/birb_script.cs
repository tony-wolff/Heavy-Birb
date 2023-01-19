using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
        if(!SceneScript.isLegacy)
        {
            myRigidBody.gravityScale = 4;
            DecreaseBirbSize();
        }
        cam = Camera.main;
        m_Animator = gameObject.GetComponentInChildren<Animator>();
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    private void DecreaseBirbSize()
    {
        transform.localScale /= 2;
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

    public void IncreaseBirbSize()
    {
        Vector3 levelOfFat = new Vector3(0.05f, 0.05f, 0);
        transform.localScale += levelOfFat;
        myRigidBody.gravityScale += 0.05f;
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
        if (!collision.gameObject.name.Equals("Cherry"))
        {
            logic.gameOver();
            birbAlive = false;
        }
    }
}
