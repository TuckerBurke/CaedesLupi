using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavNodeState { Current, Next, Previous, Unselected }

public class NavNode : MonoBehaviour
{
    private NavNodeState state = NavNodeState.Unselected; //unselected by default
    private SpriteRenderer sprite; //sprite attached to node gameobject. it is not active by default.
    private bool spriteOn = false; //bool to explictly show whether sprite is on or off. false by default.


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public NavNodeState State
    {
        get
        {
            return state;
        }

        set
        {
            switch(value)
            {
                case NavNodeState.Current:
                    sprite.sprite = Resources.Load("Assets/Resources/Debug/NavNodeSprites/NavNodeDebugCurrent.png") as Sprite; //i know this is dumb and inefficient and i should use a tilemap instead, but whatever, it's a debug sprite and we have ~1 week left.
                    return;

                case NavNodeState.Next:
                    sprite.sprite = Resources.Load("Assets/Resources/Debug/NavNodeSprites/NavNodeDebugNext.png") as Sprite;
                    return;

                case NavNodeState.Previous:
                    sprite.sprite = Resources.Load("Assets/Resources/Debug/NavNodeSprites/NavNodeDebugPrevious.png") as Sprite;
                    return;

                case NavNodeState.Unselected:
                    sprite.sprite = Resources.Load("Assets/Resources/Debug/NavNodeSprites/NavNodeDebugUnselected.png") as Sprite;
                    return;

                default:
                    return;
            }
        }
    }

    public void ToggleSprite()
    {
        if(spriteOn == false)
        {
            sprite.enabled = true;
            spriteOn = true;
        }
        else
        {
            sprite.enabled = false;
            spriteOn = false;
        }
    }

    public bool SpriteOn
    {
        get
        {
            return spriteOn;
        }

        set
        {
            if(value == true) //turn on sprite
            {
                sprite.enabled = true;
                spriteOn = true;
            }
            else //turn off sprite
            {
                sprite.enabled = false;
                spriteOn = false;
            }
        }
    }
}
