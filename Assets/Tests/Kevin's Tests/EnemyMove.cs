using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;


    private bool direction; //true is facing right, false is facing left
    private TestEnemySight sightReference;
    private float originalPos = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        sightReference = GetComponent<TestEnemySight>();
        direction = sightReference.direction;
        //Debug.Log("Direction gotten from sightReference: " + direction);
        originalPos = transform.position.x;
    }

    public void PatrollMove()
    {
        if(direction)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
        }
        
        
        if (transform.position.x > (originalPos + moveDistance) || transform.position.x < (originalPos - moveDistance))
        {
            //Debug.Log("Turn around!");
            direction = !direction;
            sightReference.TurnAround();
        }
    }

    public bool ActiveMove()
    {
        if (direction)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime * 1.5f;
        }
        else
        {
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime * 1.5f;
        }

        if (transform.position.x > (transform.position.x + moveDistance) || transform.position.x < (transform.position.x - moveDistance))
        {
            return true; //reached edge of patrol
        }
        else
        {
            return false; //did not reach edge of patrol
        }
    }
}
