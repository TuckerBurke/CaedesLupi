using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KevinMove : MonoBehaviour
{
    Rigidbody2D myBody;
    //suggest setting personal gravity scale to 2
    [SerializeField] private float horizontalMoveSpeed = 3.0f;
    [SerializeField] private float verticalMoveSpeed = 15.0f;
    //[SerializeField] private float horizontalMaxSpeed = 3.0f;
    //[SerializeField] private float verticalMaxSpeed = 3.0f;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //myBody.AddForce(Vector2.up * speedMultiplier);
            //myBody.position = new Vector2(myBody.position.x, (myBody.position.y + (1 * speedMultiplier)));
            //myBody.position = myBody.position + Vector2.up * speedMultiplier * Time.deltaTime;
            myBody.AddForce(Vector2.up * verticalMoveSpeed, ForceMode2D.Impulse); //maybe do this??
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //nothing in this case

            //myBody.AddForce(Vector2.down * speedMultiplier);
            //myBody.position = myBody.position - Vector2.up * horizontalMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            myBody.position += Vector2.left * horizontalMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            myBody.position += Vector2.right * horizontalMoveSpeed * Time.deltaTime;
        }


        //if (myBody.velocity.magnitude > horizontalMaxSpeed)
        //{
        //    //myBody.velocity = myBody.velocity.normalized * maxSpeed;
        //}
    }
}
