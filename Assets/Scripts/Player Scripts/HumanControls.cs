using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanControls : MonoBehaviour
{
    //This script handles player movement. player moves similarly to the wolf, except jump goes straight up instead of lunging. Ground detection is handled in the GroundDetection Script.
    public float runSpeed = 6.0f;
    public bool grounded;
    private bool goingRight = true;
    public bool alive = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform humanTransform;
    Animator anim;


    private Vector2 jumpForce;

    // Properties
    public bool Grounded    { get => grounded; set { grounded = value; } }
    public bool GoingRight  { get => goingRight; set { goingRight = value; } }
    public Animator Anim    { get => anim; }
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = gameObject.transform.parent.GetComponent<Transform>();
        humanTransform = gameObject.GetComponent<Transform>();
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
        jumpForce = new Vector2(0.0f, 15f);

        anim = GetComponent<Animator>();

        alive = true;
    }

    // Called when player transforms
    void OnEnable()
    {
        // If is this forms direction differs from previous
        if (goingRight != playerTransform.GetChild(0).GetComponent<WolfControls>().GoingRight)
        {
            // Toggle to opposite direction
            goingRight = playerTransform.GetChild(0).GetComponent<WolfControls>().GoingRight;

            // If previous for was facing right
            if (goingRight)
            {
                // Face right
                humanTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            // Otherwise face left
            else
            {
                humanTransform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            // If airborne
            if (!grounded)
            {
                rb.gravityScale = 2.0f;

                // Pass to state machine
                anim.SetBool("Grounded", false);
                anim.SetBool("Jump", true);
            }
            // If grounded
            else
            {
                anim.SetBool("Grounded", true);
                anim.SetBool("Jump", false);
            }

            // IF moving horizontally
            if (rb.velocity.y == 0)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Falling", false);
            }

            // If player is falling
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Falling", true);
            }

            // If player is jumping
            if(rb.velocity.y > 0)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("Falling", false);
            }

            // Player presses 'D'
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                playerTransform.Translate(new Vector3(runSpeed, 0.0f, 0.0f) * Time.deltaTime);

                // If player not heading to the right
                if (!goingRight)
                {
                    // Toggle direction tracker
                    goingRight ^= true;

                    // Face skeleton to the right
                    humanTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                // Start run animation
                anim.SetBool("Run", true);
                anim.SetBool("Idle", false);
            }

            // Player releases 'D'
            else if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)))
            {
                // Stop run animation
                anim.SetBool("Run", false);

                // Return to idle animation
                anim.SetBool("Idle", true);
            }

            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                playerTransform.Translate(new Vector3(-runSpeed, 0.0f, 0.0f) * Time.deltaTime);

                // If player is heading to the right
                if (goingRight)
                {
                    // Toggle direction tracker
                    goingRight ^= true;

                    // Face skeleton to the left
                    humanTransform.localRotation = Quaternion.Euler(0, 180, 0);
                }

                // Start run animation
                anim.SetBool("Run", true);
                anim.SetBool("Idle", false);
            }

            // Player releases 'A'
            else if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)))
            {
                // Stop run animation
                anim.SetBool("Run", false);

                // Return to idle animation
                anim.SetBool("Idle", true);
            }
    
            // Player presses 'space'
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(jumpForce, ForceMode2D.Impulse);

                // Switch to jump substate machine
                anim.SetBool("Grounded", false);
                anim.SetBool("Jump", true);
            }
        }
        // Otherwise the player is dead
        else
        {
            // Press the R key
            if (Input.GetKeyDown(KeyCode.R))
            {
                // To reset the game
                SceneManager.LoadScene("Level");
            }
        }

        // If player presses escape key
        if (Input.GetKey(KeyCode.Escape))
        {
            // Quit the game
            Application.Quit();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //alive = false;
        anim.SetBool("Idle", false);
        anim.SetBool("Dead", true);
        alive = false;

        // Trigger a popup?
    }
}
