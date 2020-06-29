using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private GameObject blackBackground;

    // Start is called before the first frame update
    void Start()
    {
        blackBackground.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(blackBackground.GetComponent<CanvasGroup>().alpha != 0.0f)
        {
            blackBackground.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
        }
    }
}
