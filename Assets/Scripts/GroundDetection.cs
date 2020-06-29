using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    // Serialized fields for all player forms
    [SerializeField] private Transform wolf;
    [SerializeField] private Transform human;

    void Start()
    {
        // Obtain references to player's other forms
        wolf = gameObject.transform.GetChild(0);
        human = gameObject.transform.GetChild(1);
    }

    // When a collision is detected
    void OnCollisionEnter2D(Collision2D theCollision)
    {
        // Set grounded property
        wolf.GetComponent<WolfControls>().Grounded = true;
        human.GetComponent<HumanControls>().Grounded = true;
    }

    // When a collision is detected
    void OnCollisionStay2D(Collision2D theCollision)
    {
        // Set grounded property
        wolf.GetComponent<WolfControls>().Grounded = true;
        human.GetComponent<HumanControls>().Grounded = true;
    }

    // When collision is removed (jumping or falling)
    void OnCollisionExit2D(Collision2D theCollision)
    {
        // Set grounded property
        wolf.GetComponent<WolfControls>().Grounded = false;
        human.GetComponent<HumanControls>().Grounded = false;
    }
}
