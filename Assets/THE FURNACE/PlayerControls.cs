using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpForce;
    public Vector2 lungeForce;
    public Vector2 backLunge;
    public bool onGround;
    bool lunged;
    bool facingForward;
    // Start is called before the first frame update
    void Start()
    {
        //refrence the rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();
        jumpForce = 10.0f;
        lungeForce = new Vector2(15.0f, jumpForce / 1.2f);
        backLunge = new Vector2(-15.0f, jumpForce / 1.2f);


        onGround = false;
        lunged = false;
        facingForward = false;
    }

    // Update is called once per frame
    void Update()
    {
        //keep object at 0 y for now
        /*if (gameObject.transform.position.y <= 0)
        {
            //when on ground, turn gravity off
            transform.position = new Vector3(gameObject.transform.position.x, 0, 0);
            onGround = true;
            rb.gravityScale = 0.0f;
        }
        else
        {
            //when not on ground, turn garvity back on anf turn bool off
            rb.gravityScale = 3.0f;
            onGround = false;
        }*/

        if(lunged == true)
        {
            while(onGround == false)
            {
                break;
            }
            if(onGround == true)
            {
                rb.velocity = new Vector2(0, 0);
                lunged = false;
            }
        }

        
        //movement with A nd D, transforms the player instead of applying a force
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(new Vector3(8.0f, 0.0f, 0.0f) * Time.deltaTime);
            facingForward = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(new Vector3(-8.0f, 0.0f, 0.0f) * Time.deltaTime);
            facingForward = false;
        }
        //if on ground, apply force
        if (Input.GetKeyDown(KeyCode.W) && onGround == true)
        {
            //set velocity to zero, and apply impulse force
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            //set velocity to zero, and apply impulse force
            if (facingForward == true)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(lungeForce, ForceMode2D.Impulse);
                lunged = true;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(backLunge, ForceMode2D.Impulse);
                lunged = true;
            }
        }
    }



}

   
