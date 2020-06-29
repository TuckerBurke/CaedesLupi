using UnityEngine;
using System.Collections;


public class fuckedWolfControls : MonoBehaviour
{
    // Animation controller variables
    private Animator anim;
    private Transform wolfTransform;
    private bool goingRight = true;

    // Rigid body and force variables
    private Rigidbody2D rb;
    [SerializeField] private float jumpForceVertical = 10.0f;
    [SerializeField] private float jumpForceHorizontal = 15f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private bool grounded = true;
    private Vector2 lungeForce;
    private Vector2 backLunge;

    void Start ()
    {
        // Get the animation controller
        anim = GetComponent<Animator>();
        wolfTransform = gameObject.transform.parent.GetComponent<Transform>();

        // Get a reference to the rigidbody
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();

        // Initialize force vectors
        lungeForce = new Vector2(jumpForceHorizontal, jumpForceVertical);
        backLunge = new Vector2(-jumpForceHorizontal, jumpForceVertical);
        rb.gravityScale = gravityScale;

    }

    void Update ()
    {
        // Player presses 'D'
        if(Input.GetKey(KeyCode.D))
        {
            Debug.Log("Keypressed");
            // If player not heading to the right
            if(!goingRight)
            {
                // Toggle direction tracker
                goingRight ^= true;

                // Face skeleton to the right
                wolfTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            // Start run animation
            anim.SetBool("Run", true);
        }

        // Player releases 'A'
        else if (Input.GetKeyUp("a"))
        {
            // Stop run animation
            anim.SetBool("Run", false);

            // Return to idle animation
            anim.SetBool("Idle", true);    
        }

        // Player presses 'A'
        if (Input.GetKey(KeyCode.A))
        {
            // If player is heading to the right
            if (goingRight)
            {
                // Toggle direction tracker
                goingRight ^= true;

                // Face skeleton to the left
                wolfTransform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            // Start run animation
            anim.SetBool("Run", true);
        }

        // Player releases 'D'
        else if (Input.GetKeyUp("d"))
        {
            // Stop run animation
            anim.SetBool("Run", false);

            // Return to idle animation
            anim.SetBool("Idle", true);
        }

        // Player presses 'S' or 'Space'
        else if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.Space))
        {
            // Start crouching animation
            anim.SetBool("Leap", false);
            anim.SetBool("Crouch", true);
        }

        // Player releases 'S' or 'Space'
        else if ((Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.Space)))
        {
            // Start leap animation
            anim.SetBool("Leap", true);
            anim.SetBool("Crouch", false); 
        }
    }

    //From unity answers grounded toggle
    //make sure u replace "floor" with your gameobject name.on which player is standing
    void OnCollisionEnter2DParent(Collision2D theCollision)
    {
        Debug.Log(theCollision.gameObject.tag);
        if (theCollision.gameObject.name == "Player")
        {
            grounded = true;
        }
    }

    //consider when character is jumping .. it will exit collision.
    void OnCollisionExit2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.name == "Player")
        {
            grounded = false;
        }
    }

}
