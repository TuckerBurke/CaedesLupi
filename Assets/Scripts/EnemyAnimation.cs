using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public GameObject player;
    public Animator anim;

    // Properties
    public Animator Anim { get => anim; }

    // Start is called before the first frame update
    void Start()
    {
        // Get the animation controller
        anim = GetComponentInChildren<Animator>();

        // Obtain a reference to the player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when a collision with player occurs
    void OnTriggerEnter2D(Collider2D col)
    {
        // If the player is in wolf form and is leaping
        if(!player.GetComponent<PlayerTransform>().Human)
        {
            
            // If the player is leaping at the enemy from behind
            if(gameObject.transform.rotation.y == player.transform.GetChild(0).transform.rotation.y)
            {
                // If facing right
                if(gameObject.transform.rotation.y == 0)
                {
                    // Then flip the enemy to the left
                    gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    // Then flip the enemy to the right
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }      
            }

            // If the wolf is leaping
            if (player.GetComponentInChildren<WolfControls>().Leaping)
            {
                // Set enemy animation bool to spearing
                anim.SetTrigger("Speared");

                // Set enemy as airborne
                anim.SetBool("Grounded", false);
            }
            // Otherwise the wolf snuck up on enemy
            else
            {
                // Set enemy animation bool to landing
                anim.SetTrigger("Snuck");
            }

            // Make enemy a child of leaping player
            gameObject.transform.parent = player.transform;

            // Enemy inherits player root location (so corpse lines up)
            gameObject.transform.position = player.transform.position;

            // Deletes the collider
            gameObject.transform.GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
