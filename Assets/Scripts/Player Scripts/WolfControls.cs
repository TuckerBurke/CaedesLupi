using UnityEngine;
using System.Collections;
using System.Runtime.Hosting;

public class WolfControls : MonoBehaviour
{
    // Animation controller variables
    public Animator anim;
    [SerializeField] private Transform wolfTransform;
    [SerializeField] private bool goingRight = true;
    [SerializeField] private float leapDuration = 0.8f;
    public bool leaping = false;
    public bool falling = false;
    public bool grounded = true;
    public bool crouching = false;
    public bool alive = true;
    public bool mauling = false;
    public int maulNumber = 8;
    GameObject currentEnemy = null;

    // Rigid body and force variables
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    public float runSpeed = 8.0f;
    [SerializeField] private float jumpForceVertical = 8.0f;
    [SerializeField] private float jumpForceHorizontal = 20.0f;
    [SerializeField] private float gravityScale = 1.0f;
    private Vector2 lungeForce;
    private Vector2 backLunge;

    // Properties
    public bool Grounded    { get => grounded; set { grounded = value; } }
    public bool GoingRight  { get => goingRight; set { goingRight = value; } }
    public bool Mauling     { get => mauling; set { mauling = value; } }
    public bool Leaping     { get => leaping; set { leaping = value; } }
    public Animator Anim    { get => anim; }

    void Start ()
    {
        // Get the animation controller
        anim = GetComponent<Animator>();
        wolfTransform = gameObject.GetComponent<Transform>();

        // Get a reference to the player and rigidbody
        playerTransform = gameObject.transform.parent.GetComponent<Transform>();
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();

        // Initialize force vectors
        lungeForce = new Vector2(jumpForceHorizontal, jumpForceVertical);
        backLunge = new Vector2(-jumpForceHorizontal, jumpForceVertical);
        rb.gravityScale = gravityScale;

        alive = true;
    }

    // Called when player transforms
    void OnEnable()
    {
        // Reset variables
        leaping = false;
        falling = false;

        // If is this forms direction differs from previous
        if (goingRight != playerTransform.GetChild(1).GetComponent<HumanControls>().GoingRight)
        {
            // Toggle to opposite direction
            goingRight = playerTransform.GetChild(1).GetComponent<HumanControls>().GoingRight;

            // If previous for was facing right
            if (goingRight)
            {
                // Face right
                wolfTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            // Otherwise face left
            else
            {
                wolfTransform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Update ()
    {
        // Things you can do if you're alive and stuff
        if(alive && !leaping && !crouching && !falling && !mauling)
        {
            // Player presses 'D'
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                // If player not heading to the right
                if (!goingRight)
                {
                    // Toggle direction tracker
                    goingRight ^= true;

                    // Face skeleton to the right
                    wolfTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                // Start run animation
                anim.SetBool("Run", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Leap", false);

                // Move the player
                playerTransform.Translate(new Vector3(runSpeed, 0.0f, 0.0f) * Time.deltaTime);
            }

            // Player releases 'D'
            else if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)))
            {
                // Stop run animation
                anim.SetBool("Run", false);

                // Return to idle animation
                anim.SetBool("Idle", true);
                anim.SetBool("Leap", false);
            }

            // Player presses 'A'
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
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
                anim.SetBool("Idle", false);
                anim.SetBool("Leap", false);

                // Move the player
                playerTransform.Translate(new Vector3(-runSpeed, 0.0f, 0.0f) * Time.deltaTime);
            }

            // Player releases 'A'
            else if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)))
            {
                // Stop run animation
                anim.SetBool("Run", false);

                // Return to idle animation
                anim.SetBool("Idle", true);
                anim.SetBool("Leap", false);
            }

            // Player presses 'S' or 'Space' and is on the ground
            else if (Input.GetKey(KeyCode.Space) && grounded)
            {
                // Start crouching animation
                anim.SetBool("Leap", false);
                anim.SetBool("Crouch", true);
                crouching = true;
            }

        }

        // Player releases 'S' or 'Space'
        if (Input.GetKeyUp(KeyCode.Space) && grounded && crouching && !mauling)
        {
            // Start leap animation
            rb.gravityScale = 1.0f;
            anim.SetBool("Leap", true);
            anim.SetBool("Crouch", false);
            leaping = true;
            crouching = false;

            // If the player is facing right
            if (goingRight)
            {
                //set velocity to zero, and apply positive force
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(lungeForce, ForceMode2D.Impulse);
            }

            // If the player is facing left
            else
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(backLunge, ForceMode2D.Impulse);
            }

            // Call method after leap animation
            Invoke("CompletedLeaping", leapDuration);
        }

        // If feet are on ground
        if(grounded)
        {
            // Not falling
            falling = false;
        }

        if (mauling && !leaping)
        {
            if (Input.GetMouseButtonDown(0) && (maulNumber > 0))
            {
                // Call animation clip
                anim.SetTrigger("Bite");

                // Decrement remaining bites
                maulNumber = maulNumber - 1;
            }
            else if (maulNumber == 0)
            {
                // Leave behind corpse HACK
                Destroy(transform.parent.GetChild(3).GetComponent<BoxCollider2D>());
                transform.parent.GetChild(3).parent = null;
                currentEnemy = null;

                // Reset variables
                anim.SetBool("Crouch", false);
                mauling = false;
                anim.SetBool("Mauling", false);
            }
        }

        // If player presses escape key
        if(Input.GetKey(KeyCode.Escape))
        {
            // Quit the game
            Application.Quit();
        }
    }

    // Called when a collision with enemy occurs
    void OnTriggerEnter2D(Collider2D col)
    {
        // Set mauling state
        mauling = true;

        // Determin how many bites to kill (like Hotline Miami)
        maulNumber = Random.Range(1, 6);

        // Obtain reference to the attacked enemy
        currentEnemy = col.gameObject;

        // Set animation bools
        anim.SetBool("Run", false);
        anim.SetBool("Idle", true);
        anim.SetBool("Mauling", true);
        
        // Begin attack stance
        anim.SetBool("Crouch", true);
    }

    // Runs at the end of the leap animation
    void CompletedLeaping()
    {
        // End leap animation
        anim.SetBool("Leap", false);
        leaping = false;

        // If in flying combat
        if (mauling)
        {
            // Begin enemy landing animation
            currentEnemy.GetComponent<EnemyAnimation>().Anim.SetBool("Grounded", true);

            // Begin attack stance
            anim.SetBool("Crouch", true);
        }

        // If player is falling
        if (rb.velocity.y < 0)
        {
            // Flag as falling
            falling = true;
        }
    }
}
