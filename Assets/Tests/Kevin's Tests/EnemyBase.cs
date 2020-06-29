using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author(s): Kevin Dolan.
 * Relationship(s): Inheritated by the child classes EnemyHuman, EnemyWolf, EnemyHunter.
 * Purpose: Make it easier to develop enemy classes with common features.
 * How It Work: idk
 */

//enums to show enemy state and help with state machine
public enum EnemyState { Patrolling, AttackWindup, Attacking, AttackWinddown, ActiveSearch, IdleSearch, Mauled, Dead }

public abstract class EnemyBase : MonoBehaviour
{
    //Configuration Variables: These variables are meant to be tweaked and edited in the Unity inspector.
    [Tooltip("How quickly the enemy moves while patrolling.")]
    [SerializeField] protected float patrolSpeed = 0.0f; //how quickly the enemy moves while they are patrolling and haven't seen the player.
    [Tooltip("How far away the enemy can see the player while looking in the right direction.")]
    [SerializeField] protected float visionLength = 3.0f; //how far the enemy can see.
    [Tooltip("The arc of the vision cone of the enemy. Angle is in degrees.")]
    [SerializeField] protected float visionArc = 70.0f; //the arc of the cone that the enemy "sees". in degrees. 
    [Tooltip("How far away the enemy can hear the player.")]
    [SerializeField] protected float hearingRange = 2.0f; //how far away the enemy can detect the player by "hearing" (pure distance check).
    [Tooltip("How quickly the enemy moves when the player is spotted.")]
    [SerializeField] protected float searchSpeed = 0.0f; //how quickly the enemy moves after they have seen the player
    [Tooltip("How long the enemy waits after spotting and losing the player before resuming patrolling.")]
    [SerializeField] protected float idleSearchDuration = 0.0f; //how long it takes the enemy to go back to normal patrolling after losing sight of the player
    [Tooltip("")]
    [SerializeField] protected float attackWindupDuration = 0.0f; //how long the enemy waits before firing/lunging at the player
    [SerializeField] protected float attackWinddownDuration = 0.0f; //how long the enemy waits AFTER firing/lunging at the player before doing anything else

    //i'm not sure if i want to use these last two variables, but i'll put there here in case i want to
    //[SerializeField] protected float patrolTurnDelay = 0.0f; //how long the enemy waits at the edge of their patrol before turning around
    //[SerializeField] protected float detectDelay = 0.0f; //how frequently the enemy actually calls its detection methods. i would only use this delay if checking every frame caused the game to lag.


    //State Variables: These are used to keep track of necessary information while the bastard is running.    
    protected float attackWindupTimer = 0.0f;
    protected float attackWinddownTimer = 0.0f;
    protected float idleSearchTimer = 0.0f;
    protected bool direction = true; //direction that the enemy is facing. true = right, false = left.
    private EnemyState state = EnemyState.Patrolling; //initialized as Patrolling. this is private by so that child classes can only change the state via the specialized property. see that property's comment for why.

    //debug state variables
    protected bool canAct = true; //lets enemy do literally anything, e.g. move, see, attack
    protected bool canSee = true; //lets enemy see player
    protected bool canHear = true; //lets enemy hear player

    //i might use these
    //protected float patrolTurnTimer = 0.0f; //timer that keeps track of patrol turn delay time
    //protected float playerCheckTimer = 0.0f; //timer that keeps track of time between detection checks


    //Holder Variables: These are used to avoid instantiating new spaces in memory for a variable that is going to be generated and used the same way every time. 
    //The alternative to using holder variables is instantiating a new variable every time a method is called, which is inefficient and may cause frame rate issues when called by each enemy in the level.
    protected Vector2 directionEnemyToPlayer = Vector2.zero;

    //Debug Variables: These are used to show information about the enemy, probably shown via ui attached to enemy sprite
    protected EnemyState previousState = EnemyState.Patrolling; //keep track of what the last state the enemy was in. useful in case the enemy starts doing weird shit and i want to know what it was doing before it starting being weird.
    protected bool showAll = false;
    protected bool showVision = false;
    protected bool showHearing = false;
    protected bool showSearchIdleTimer = false;
    protected bool showAttackWindupTimer = false;
    protected bool showAttackWinddownTimer = false;
    protected bool showAttack = false;
    protected bool showCurrentState = false;
    protected bool showPreviousState = false;
    protected bool showDirection = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //handle most of the state machine behavior. implemented differently by each child class.
    protected abstract void MainLoop();

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
            if(direction == true && directionEnemyToPlayer.x < 0) //if enemy is facing right (->) and direction towards player is going left (<-), disqualify
            {
                return;
            }
            else if(direction == false && directionEnemyToPlayer.x > 0) //if enemy is facing left (<-) and direction towards player is going right (->), disqualify
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


    //kill enemy
    public void Die()
    {
        State = EnemyState.Dead;
    }

    //resurrect enemy and set them to Patrolling
    public void Res()
    {
        State = EnemyState.Patrolling;
    }

    //public property to get the enemy's state
    public EnemyState GetState()
    {
        return state;
    }

    //internal property which has normal Get part but has special Set part which saves the old value of state to a variable called previous state.
    protected EnemyState State
    {
        get
        {
            return state;
        }

        set
        {
            previousState = state;
            state = value;
        }
    }
}
