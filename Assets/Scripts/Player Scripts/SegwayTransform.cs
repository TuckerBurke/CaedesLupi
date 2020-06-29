using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegwayTransform : MonoBehaviour
{
    public bool goingRight = true;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Called when player transforms
    void OnEnable()
    {
        // If is this forms direction differs from previous
        if (goingRight != playerTransform.GetChild(1).GetComponent<HumanControls>().GoingRight)
        {
            // Toggle to opposite direction
            goingRight = playerTransform.GetChild(1).GetComponent<HumanControls>().GoingRight;

            // If previous for was facing right
            if (goingRight)
            {
                // Face right
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            // Otherwise face left
            else
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
