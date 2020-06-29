using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject player;
    public int transformsLeft;

    [SerializeField] private float lastUseRechargeTimerDefault;
    private float lastUseRechargeTimer;

    [SerializeField] private GameObject useOne;
    [SerializeField] private GameObject useTwo;
    [SerializeField] private GameObject useThree;

    // Start is called before the first frame update
    void Start()
    {
        lastUseRechargeTimer = lastUseRechargeTimerDefault;
    }

    // Update is called once per frame
    void Update()
    {
        transformsLeft = player.GetComponent<PlayerTransform>().numOfTransformsLeft;

        if (transformsLeft == 3)
        {
            useOne.SetActive(true);
            useOne.GetComponent<CanvasGroup>().alpha = 1f;
            useTwo.SetActive(true);
            useThree.SetActive(true);
        }
        else if(transformsLeft == 2)
        {
            useOne.SetActive(true);
            useOne.GetComponent<CanvasGroup>().alpha = 1f;
            useTwo.SetActive(true);
            useThree.SetActive(false);
        }
        else if(transformsLeft == 1)
        {
            useOne.SetActive(true);
            useOne.GetComponent<CanvasGroup>().alpha = 1f;
            useTwo.SetActive(false);
            useThree.SetActive(false);
        }
        else if(transformsLeft == 0)
        {
            useOne.SetActive(true);
            useOne.GetComponent<CanvasGroup>().alpha = 0.2f;
            useTwo.SetActive(false);
            useThree.SetActive(false);
        }

        if(transformsLeft <= 0)
        {
            lastUseRechargeTimer -= Time.deltaTime;

            if(lastUseRechargeTimer <= 0)
            {
                player.GetComponent<PlayerTransform>().numOfTransformsLeft = 1;
                lastUseRechargeTimer = lastUseRechargeTimerDefault;
            }
        }

    }
}
