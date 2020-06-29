using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length);
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject enemy in enemies)
        {
            TestEnemySight sight = enemy.GetComponent<TestEnemySight>();
            EnemyMove move = enemy.GetComponent<EnemyMove>();

            switch (sight.state)
            {
                case EnemyState.Patrolling:
                    move.PatrollMove();
                    if(sight.AngleCheck(player) || sight.HearingCheck(player)) //if enemy sees player
                    {
                        sight.state = EnemyState.AttackWindup;
                    }
                    
                    break;

                case EnemyState.AttackWindup:
                    if(sight.AttackWindup())
                    {
                        sight.state = EnemyState.Attacking;
                    }
                    break;

                case EnemyState.Attacking:
                    if(sight.AngleCheck(player))
                    {
                        //PLAYER IS KILLED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //Destroy(player); //this causes hella errors, please delete this and replace it with appropriate reaction
                        Debug.Log("I KILL YOU!");

                        sight.state = EnemyState.Patrolling;
                    }
                    else
                    {
                        sight.state = EnemyState.AttackWinddown;
                    }
                    break;

                case EnemyState.AttackWinddown:
                    if(sight.AttackWinddown())
                    {
                        sight.state = EnemyState.ActiveSearch;
                    }
                    break;

                case EnemyState.ActiveSearch:
                    if (sight.AngleCheck(player))
                    {
                        sight.state = EnemyState.AttackWindup;
                    }
                    else if(move.ActiveMove())
                    {
                        sight.state = EnemyState.IdleSearch;
                    }

                    break;

                case EnemyState.IdleSearch:
                    if(sight.AngleCheck(player))
                    {
                        sight.state = EnemyState.AttackWindup;
                    }
                    else if(sight.IdleSearch())
                    {
                        sight.state = EnemyState.Patrolling; //go back to normal patrolling
                    }
                    break;

                case EnemyState.Mauled:
                    //maul animation idk
                    break;

                case EnemyState.Dead:
                default:
                    //do nothing cause you dead
                    break;
            }
        }
    }

    //make the enemy behave
    private void EnemyBehavior(GameObject enemy)
    {
        //in the normal state (patrolling), enemies SightCheck, HearingCheck, and Patrol

    }
}
