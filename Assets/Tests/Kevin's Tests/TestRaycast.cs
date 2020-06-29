using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRaycast : MonoBehaviour
{
    bool go = false; //don't start drawing at first

    Vector2 mousePos; //holder variable
    Vector2 directionPlayerToMouse; //holder variable

    //Text theText; //reference to text object

    // Start is called before the first frame update
    void Start()
    {
        //theText = GetComponent<UIFollow>().debugText;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            go = !go; //invert on/off
        }

        if(go)
        {
            //get the position of the mouse on the screen in world space and use it for the Raycast
            mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            directionPlayerToMouse =  mousePos - (Vector2)transform.position; //direction vector from point a (player) to point b (mouse): b - a 

            //to get the raycast to NOT collide with the casting object, it seems that you MUST set the raycasting object be on a different layer than the raycast! probably obvious, but you just gotta do it.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionPlayerToMouse.normalized, directionPlayerToMouse.magnitude);
            if(hit.collider != null)
            {
                //theText.text = hit.transform.name;
                //theText.text += "\n" + transform.position;
                //theText.text += "\n" + directionPlayerToMouse;
                //theText.text += "\n" + directionPlayerToMouse.normalized;
                //theText.text += "\n" + directionPlayerToMouse.magnitude;
                hit.collider.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            //Vector3 raycast

            Debug.DrawRay(transform.position, directionPlayerToMouse, Color.green);
        }
    }
}
