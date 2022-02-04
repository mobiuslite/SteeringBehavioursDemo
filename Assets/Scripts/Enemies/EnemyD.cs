using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
public class EnemyD : Enemy
{
    Wander wander;

    [SerializeField]
    float wanderTime = 6.0f;
    float wanderTimeElapsed = 0.0f;

    [SerializeField]
    float idleTime = 3.0f;
    float idleTimeElapsed = 0.0f;
    void Start()
    {
        base.Create();

        wander = GetComponent<Wander>();
        //wander.SetTarget(target);

        wander.enabled = true;
        render.material.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        if (wander.wanderEnabled)
        {
            wanderTimeElapsed += Time.deltaTime;
            if(wanderTimeElapsed >= wanderTime)
            {
                //Set to idle
                wanderTimeElapsed = 0.0f;
                wander.wanderEnabled = false;
                //wander.StopMoving();

                render.material.color = Color.yellow;
            }
        }
        else
        {
            idleTimeElapsed += Time.deltaTime;
            if(idleTimeElapsed >= idleTime)
            {
                //Set to wander
                idleTimeElapsed = 0.0f;
                wander.wanderEnabled = true;

                render.material.color = Color.cyan;
            }
        }
    }
}
