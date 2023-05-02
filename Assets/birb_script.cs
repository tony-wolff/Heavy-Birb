using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class birb_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public GameObject echo;
    List<GameObject> echoes;
    public float flapStrength;
    public float dashStrength;
    public float delayDash;
    float timer;
    float elapsedTime;
    public LogicManagerScript logic;
    public bool birbAlive = true;
    private Camera cam;
    public SpriteRenderer mySprite;
    private Animator m_Animator;
    private float maxSize = 1.5f;
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
        timer = delayDash;
        echoes = new List<GameObject>();
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
        if (transform.localScale.x < maxSize)
        {
            Vector3 fatnessToAdd = new Vector3(0.05f, 0.05f, 0);
            transform.localScale += fatnessToAdd;
            myRigidBody.gravityScale += 0.05f;
        }
    }

    // Update is called once per frames
    void Update()
    {
        if (IsOffScreen())
        {
            logic.gameOver();
            birbAlive = false;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && birbAlive)
        {
            myRigidBody.velocity = Vector2.up * flapStrength;
            m_Animator.SetTrigger("Flapping");
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && birbAlive && timer >= delayDash)
        {
            elapsedTime = 0;
            StartCoroutine(Echo());
            myRigidBody.velocity = Vector2.right * dashStrength;
            timer = 0;
        }
        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;
    }

    public IEnumerator Echo()
    {
        while (true)
        {
            GameObject e = Instantiate(echo, transform.position, transform.rotation);
            e.transform.localScale = transform.localScale;
            echoes.Add(e);
            Debug.Log(elapsedTime);
            yield return new WaitForSeconds(0.05f);
            if (elapsedTime >= 0.5f)
            {
                foreach(GameObject ec in echoes)
                {
                    Destroy(ec);
                }
                echoes.Clear();
                yield break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //logic.gameOver();
        //birbAlive = false;
    }
}
