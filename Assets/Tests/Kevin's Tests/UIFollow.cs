using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    public Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 worldToScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        debugText.transform.position = worldToScreenPos;
    }
}
