using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPotionPickup : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position,gameObject.transform.position) <= 0.5f)
        {
            player.GetComponent<PlayerTransform>().numOfTransformsLeft = 3;
            Destroy(gameObject);
        }
    }
}
