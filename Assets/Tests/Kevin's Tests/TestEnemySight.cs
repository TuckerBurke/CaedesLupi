using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySight : MonoBehaviour
{
    [Header("Detection Settings")]    
    [SerializeField] protected float visionLength = 3.0f; //how far the enemy can see

    [Tooltip("Angle in degrees.")]
    [SerializeField] protected float visionArc = 70.0f; //the arc of the cone that the enemy "sees". units are radians. 

    [SerializeField] protected float hearingRange = 0.0f; //how far away the enemy can detect the player from hearing

    [SerializeField] private float idleSearchTime = 1.0f;
    private float idleSearchTimer;
    [SerializeField] private float attackWindupTime = 1.0f;
    private float attackWindupTimer;
    [SerializeField] private float attackWinddownTime = 1.0f;
    private float attackWinddownTimer;

    //state/holder variables
    private Vector2 directionEnemyToPlayer = Vector2.zero;
    [HideInInspector] public bool direction = true; //direction that the enemy is facing. true = right, false = left.
    public EnemyState state = EnemyState.Patrolling; //initialized as Patrolling. 

    private bool goMouse = false; //do or don't raycast from this to mouse
    private bool goPlayer = false; //do or don't raycast from this to player
    private Vector2 mousePos; //holder variable
    private Vector2 directionTowardMouse; //holder variable
    private float theAngle; //angle between forward vector and another vector
    //[SerializeField] private GameObject thePlayer; //reference to player
    private Vector2 directionTowardPlayer; //holder variable
    

    private Color alternatingColor = Color.yellow;
    private float colorAlternateTime = 1.0f;
    private float colorAlternateTimer = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.rotation.eulerAngles.y >= 350.0f && transform.rotation.eulerAngles.y <= 10.0f)
        {
            Debug.Log("Facing right!");
            direction = true; //facing right
        }
        else if(transform.rotation.eulerAngles.y >= 170.0f && transform.rotation.eulerAngles.y <= 190.0f)
        {
            Debug.Log("Facing left!");
            direction = false; //facing left
        }

        idleSearchTimer = idleSearchTime;
        attackWindupTimer = attackWindupTime;
        attackWinddownTimer = attackWinddownTime;
    }

    // Update is called once per frame
    void Update()
    {
        //AngleCheck(thePlayer);

       // HearingCheck(thePlayer);

        //AlternateColor();
    }


    //works in principle
    public bool HearingCheck(GameObject player)
    {
        Vector2 distanceBetweenEnemyAndPlayer = (Vector2)player.transform.position - new Vector2(transform.position.x, transform.position.y + 1.5f); //vector towards player

        //ray toward player, length of hearingRange
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.5f), distanceBetweenEnemyAndPlayer.normalized * hearingRange, Color.red);

        if (distanceBetweenEnemyAndPlayer.magnitude < hearingRange) //if the distance between the enemy and the player is less than the hearing range, the enemy hears the player
        {
            return true; //player is within hearing range
        }
        else
        {
            return false; //player is not within hearing range
        }
    }

    //works in principle
    //THIS SHIT ACTUALLY FUCKING WORKS YEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH
    public bool AngleCheck(GameObject player)
    {
        directionTowardPlayer = (Vector2)player.transform.position - new Vector2(transform.position.x, transform.position.y + 1.5f); //direction vector from point a (this) to point b (player): b - a 

        //to get the raycast to NOT collide with the casting object, it seems that you MUST set the raycasting object be on a different layer than the raycast! probably obvious, but you just gotta do it.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionTowardPlayer.normalized, visionLength);

        //the angle between the direction vector pointing towards the player's position and the right vector of this gameobject.
        theAngle = Vector2.Angle(directionTowardPlayer, (Vector2)transform.right);

        if(theAngle < visionArc/2) //if the angle between the player and the right vector is within the arc of vision of the enemy, continue checking hitscan
        {
            if (hit.collider != null && hit.collider.tag == "Player") //HIT HIT HIT
            {
                //hit.collider.GetComponent<SpriteRenderer>().color = alternatingColor; //debug

                return true;
                
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
            
        ////ray toward player, length of visionLength
        //Debug.DrawRay(transform.position, directionTowardPlayer.normalized * visionLength, Color.cyan);
            
        ////ray of the right vector of the this gameobject
        //Debug.DrawRay(transform.position, transform.right, Color.blue);

        ////from the position of the this gameobject, draw a ray forward, then rotate it by half the vision arc (e.g. 30 degrees) up OR down, then make the ray as long as visionLength.
        //Debug.DrawRay(transform.position, Quaternion.AngleAxis(visionArc/2, Vector3.forward) * transform.right * visionLength, Color.blue);
        //Debug.DrawRay(transform.position, Quaternion.AngleAxis(-visionArc/2, Vector3.forward) * transform.right * visionLength, Color.blue);

        
    }

    public bool IdleSearch()
    {
        idleSearchTimer -= Time.deltaTime;

        if(idleSearchTimer > 0)
        {
            return false; //not done idly searching
        }
        else
        {
            idleSearchTimer = idleSearchTime; //reset timer for next time
            return true; //if done idly searching
        }
    }

    public bool AttackWindup()
    {
        attackWindupTimer -= Time.deltaTime;

        if (attackWindupTimer > 0)
        {
            return false; //not done idly searching
        }
        else
        {
            attackWindupTimer = attackWindupTime; //reset timer for next time
            return true; //if done idly searching
        }
    }

    public bool AttackWinddown()
    {
        attackWinddownTimer -= Time.deltaTime;

        if (attackWinddownTimer > 0)
        {
            return false; //not done idly searching
        }
        else
        {
            attackWinddownTimer = attackWinddownTime; //reset timer for next time
            return true; //if done idly searching
        }
    }

    //irrelevant
    private void SimpleRaycastCheck(GameObject player)
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            goMouse = !goMouse; //invert on/off
            goPlayer = false; //only one raycast check can be on at a time
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            goPlayer = !goPlayer; //invert on/off
            goMouse = false; //only one raycast check can be on at a time
        }

        if (goMouse)
        {
            //get the position of the mouse on the screen in world space and use it for the Raycast
            mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            directionTowardMouse = mousePos - (Vector2)transform.position; //direction vector from point a (this) to point b (mouse): b - a 

            //to get the raycast to NOT collide with the casting object, it seems that you MUST set the raycasting object be on a different layer than the raycast! probably obvious, but you just gotta do it.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionTowardMouse.normalized, directionTowardMouse.magnitude);
            if (hit.collider != null)
            {
                //theText.text = hit.transform.name;
                //theText.text += "\n" + transform.position;
                //theText.text += "\n" + directionPlayerToMouse;
                //theText.text += "\n" + directionPlayerToMouse.normalized;
                //theText.text += "\n" + directionPlayerToMouse.magnitude;
                hit.collider.GetComponent<SpriteRenderer>().color = Color.white;
            }

            Debug.DrawRay(transform.position, directionTowardMouse, Color.green);
            Debug.DrawRay(transform.position, transform.right, Color.blue);

            theAngle = Vector2.Angle(directionTowardMouse, (Vector2)transform.right);

        }

        if (goPlayer)
        {
            //get the position of the mouse on the screen in world space and use it for the Raycast
            mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            directionTowardPlayer = (Vector2)player.transform.position - (Vector2)transform.position; //direction vector from point a (this) to point b (player): b - a 

            //to get the raycast to NOT collide with the casting object, it seems that you MUST set the raycasting object be on a different layer than the raycast! probably obvious, but you just gotta do it.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionTowardPlayer.normalized, directionTowardPlayer.magnitude);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<SpriteRenderer>().color = Color.green;
            }

            Debug.DrawRay(transform.position, directionTowardPlayer, Color.cyan);
            Debug.DrawRay(transform.position, transform.right, Color.blue);

            theAngle = Vector2.Angle(directionTowardPlayer, (Vector2)transform.right);

        }
    }

    //irrelevant
    public void LineOfSight(GameObject player)
    {
        //player.transform.GetComponent<BoxCollider2D>().
        directionEnemyToPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
        //direction vector from point a (player) to point b (mouse): b - a 

        //to get the raycast to NOT collide with the casting object, it seems that you MUST set the raycasting object be on a different layer than the raycast! probably obvious, but you just gotta do it.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionEnemyToPlayer.normalized, directionEnemyToPlayer.magnitude);
        if (hit.collider != null)
        {
            //theText.text = hit.transform.name;
            //theText.text += "\n" + transform.position;
            //theText.text += "\n" + directionPlayerToMouse;
            //theText.text += "\n" + directionPlayerToMouse.normalized;
            //theText.text += "\n" + directionPlayerToMouse.magnitude;
            hit.collider.GetComponent<SpriteRenderer>().color = Color.yellow;

            //check for disqualification of valid line of sight

            //if enemy is looking away from player, disqualify
            if (direction == true && directionEnemyToPlayer.x < 0) //if enemy is facing right (->) and direction towards player is going left (<-), disqualify
            {
                return;
            }
            else if (direction == false && directionEnemyToPlayer.x > 0) //if enemy is facing left (<-) and direction towards player is going right (->), disqualify
            {
                return;
            }

            //if within vision range, continue checking
            if (directionEnemyToPlayer.x < visionLength)
            {

            }
        }

        //Vector3 raycast

        Debug.DrawRay(transform.position, directionEnemyToPlayer, Color.green);

    }

    //irrelevant
    //helper debug method to make a color variable change back and forth over time. purely a debugging visibility method.
    private void AlternateColor()
    {
        colorAlternateTimer -= Time.deltaTime; //count down timer

        if(colorAlternateTimer <= 0.0f) //when timer runs out, switch the color
        {
            if(alternatingColor == Color.yellow) //if color was yellow, turn it to white
            {
                alternatingColor = Color.white;
            }
            else //if color was not yellow (white), turn it to yellow
            {
                alternatingColor = Color.yellow;
            }

            colorAlternateTimer = colorAlternateTime; //reset timer
        }
    }

    //use this method whenever the enemy should turn around
    public void TurnAround() //........ bright eyes.......
    {
        direction = !direction; //invert direction.

        transform.Rotate(0.0f, 180.0f, 0.0f); //turn around the gameobject.
    }
}
