using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNodeMeshScript : MonoBehaviour
{
    private NavNode[] nodes;

    //these actually belong on the enemy script...
    //************** MOVE **************
    private NavNode currentNode;
    private NavNode nextNode;
    private NavNode previousNode;

    private bool spritesOn = false; //bool to control whether sprites are shown or not. false by default.
    private bool linesOn = false; //bool to control whether debug lines are drawn between nodes. false by default.

    //holder attributes
    private float colorOffset;
    private Color nodeOrderLineColor;
    private int showSomeDebugInfo = 0;
    private bool showNoderOrderOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        nodes = GetComponentsInChildren<NavNode>();

        //do some checks to make sure this is a valid nav mesh of nodes.
        if(nodes.Length == 0) //check if nav mesh has any nodes in it at all. if not, throw an exception.
        {
            Debug.LogError("This NavNodeMesh has no NavNodes in it!", transform);
            
        }
        else if(nodes.Length == 1) //check if nav mesh has only one node in it. if not, throw an exception.
        {
            Debug.LogError("This NavNodeMesh has only ONE NavNode in it!", transform);
            
        }

        //do a check to make sure the nodes in the array are ordered from left to right.
        float holderA = nodes[0].transform.position.x;
        float holderB;

        int i = 1; //note that this do-while loop starts at i = 1, not i = 0.
        do
        {
            holderB = nodes[i].transform.position.x;

            if (holderB < holderA) //if a node later in the array is behind a node earlier in the array, throw an exception.
            {
                Debug.LogError("NavNodes in this NavNodeMesh are not \"childed\" in order from left to right transform position!", transform);
                
            }

            holderA = holderB;

            i++;
        } while (i < nodes.Length);


        colorOffset = Mathf.Lerp(0, 1, 1 / (float)nodes.Length); //this is used in the ShowNodeOrder debugging method
    }

    // Update is called once per frame
    void Update()
    {
        //debug thing
        if(Input.GetKeyDown(KeyCode.Y))
        {
            showSomeDebugInfo++;
            ShowFirstNode();
        }

        switch(showSomeDebugInfo)
        {
            case 0:
                //show nothing
                break;
            case 1:
                if(showNoderOrderOnce)
                {
                    PrintNodeOrder();
                    showNoderOrderOnce = false;
                }
                break;
            case 2:
                SpritesOn = true; //this is dumb and inconsistent way of turning all the sprites on compared to how i turn on the ShowNodeOrder method
                break;
            case 3:
                linesOn = true; //this is a dumb and inconsistent way to turning on all the lines compared to hwo i turn on the sprites
                break;
            default:
                showSomeDebugInfo = 0;
                SpritesOn = false;
                linesOn = false;
                showNoderOrderOnce = true;
                break;
        }
        
        if(linesOn)
        {
            ShowNodeOrder();
        }
    }

    #region debug stuff

    public void ShowFirstNode()
    {
        Debug.Log(nodes[0].transform.position);
    }

    public void ToggleSprites()
    {
        foreach(NavNode node in nodes)
        {
            node.ToggleSprite();
        }

        spritesOn = !spritesOn; //invert value of spritesOn
    }

    public bool SpritesOn
    {
        get
        {
            return spritesOn;
        }

        set
        {
            if(value == true)
            {
                foreach(NavNode node in nodes)
                {
                    node.SpriteOn = true;
                }
                spritesOn = true;
            }
            else
            {
                foreach (NavNode node in nodes)
                {
                    node.SpriteOn = false;
                }
                spritesOn = false;
            }
        }
    }

    public bool LinesOn
    {
        get
        {
            return linesOn;
        }

        set
        {
            linesOn = value;
        }
    }

    public void PrintNodeOrder()
    {
        foreach(NavNode node in nodes)
        {
            Debug.Log(node.name);
        }
    }

    public void ShowNodeOrder()
    {

        for (int i = 1; i < nodes.Length; i++) //note that this for loop starts at i = 1, not i = 0.
        {
            nodeOrderLineColor = new Color(1.0f, (i - 1) * colorOffset, (i - 1) * colorOffset); //generate a color that is a shade a red depending on how many nodes there are in the list and what iteration this loop is on

            Debug.DrawLine(nodes[i - 1].transform.position, nodes[i].transform.position, nodeOrderLineColor);
        }        
    }

    #endregion

    #region properties for three main nodes
    public NavNode CurrentNode
    {
        get
        {
            return currentNode;
        }
    }

    public NavNode NextNode
    {
        get
        {
            return nextNode;
        }
    }

    public NavNode PreviousNode
    {
        get
        {
            return previousNode;
        }
    }
    #endregion


}
