using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{   
    // This Script handles player transformations. The player begins as a human and can press T to transform into a wolf. The wolf and human children are disabled when not in use.

    public GameObject wolf;
    public GameObject human;
    public GameObject segway;

    private float transformDuration = 0.333f;
    public bool isHuman;
    public int numOfTransformsLeft;

    // Properties
    public bool Human { get => isHuman; set { isHuman = value; } }

    // Start is called before the first frame update
    void Start()
    {
        // Player starts out in human form
        isHuman = true;

        // Obtain references to player objects
        //human = GameObject.Find("human");
        //wolf = GameObject.Find("wolf");
        //segway = GameObject.Find("segway");

        // Initialize the number of transforms allowed
        numOfTransformsLeft = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //press T to transform
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyUp(KeyCode.T)) && !wolf.GetComponent<WolfControls>().Mauling)
        {
            // If in human form with charges remaining
            if (isHuman && numOfTransformsLeft > 0)
            {
                // Begin the animation sequence
                gameObject.transform.GetChild(1).GetComponent<HumanControls>().Anim.SetTrigger("Transform");
                segway.SetActive(true);

                // Call method to swap objects (after animation)
                Invoke("SwapObjects", (transformDuration));

                isHuman = false;
                numOfTransformsLeft--;
            }
            // If in wolf form with charges remaining
            else if (!isHuman && numOfTransformsLeft > 0)
            {
                // Begin the animation sequence
                gameObject.transform.GetChild(0).GetComponent<WolfControls>().Anim.SetTrigger("Transform");

                // Call method to swap objects (after animation)
                Invoke("SwapObjects", transformDuration);

                isHuman = true;
                numOfTransformsLeft--;
            }
        }

    }

    // Where the physical game objects actually swap
    void SwapObjects()
    {
        if (isHuman == false && numOfTransformsLeft >= 0)
        {
            human.SetActive(false);
            segway.SetActive(false);
            wolf.SetActive(true);
        }
        else if (isHuman == true && numOfTransformsLeft >= 0)
        {
            human.SetActive(true);
            wolf.SetActive(false);
        }
    }
}
