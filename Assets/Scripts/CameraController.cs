using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float wolfSpeed;
    private float humanSpeed;
    public bool grounded;

    private GameObject wolf;
    private GameObject human;
    private GameObject segway;

    [SerializeField] private float verticleCameraOffset = 2.0f;

    public bool Grounded { set { grounded = value; } }
    
    // Start is called before the first frame update
    void Start()
    {
        wolfSpeed = 8.0f;   // GetComponentInChildren<WolfControls>().runSpeed;
        humanSpeed = 6.0f;  // GetComponentInChildren<HumanControls>().runSpeed;

        human = GameObject.Find("human");
        wolf = GameObject.Find("wolf");
        segway = GameObject.Find("segway");

        // Set initial player form as human (otherwise breaks camera)
        wolf.SetActive(false);
        segway.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is a certain distance left of the camera
        if(transform.position.x <= Camera.main.transform.position.x - 1)
        {
            // Move the camera with the player
            if (gameObject.GetComponent<PlayerTransform>().isHuman == false)
            {
                Camera.main.transform.Translate(new Vector3(-(wolfSpeed-1), 0, 0) * Time.deltaTime);
            }
            //hopefully if player lunging
            if (gameObject.GetComponent<PlayerTransform>().isHuman == false && grounded == false)
            {
                Camera.main.transform.Translate(new Vector3(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0, 0) * Time.deltaTime);
            }
            else
            {
                Camera.main.transform.Translate(new Vector3(-(humanSpeed-1), 0, 0) * Time.deltaTime);
            }
            
        }

        // If the player is a certain distance right of the camera
        if (transform.position.x >= Camera.main.transform.position.x + 1)
        {
            // Move the camera with the player
            if (gameObject.GetComponent<PlayerTransform>().isHuman == false)
            {
                Camera.main.transform.Translate(new Vector3((wolfSpeed-1), 0, 0) * Time.deltaTime);
            }
            else
            {
                Camera.main.transform.Translate(new Vector3((humanSpeed-1), 0, 0) * Time.deltaTime);
            }
        }

        // Cap the camera from moving too far to the left (so world boundary to the left
        if(Camera.main.transform.position.x <= 0)
        {
            Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }

        // Camera follows the vertical movement of the player
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + verticleCameraOffset, Camera.main.transform.position.z);
    }
}
