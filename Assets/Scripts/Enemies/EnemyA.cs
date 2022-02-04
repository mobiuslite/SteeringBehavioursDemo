using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seek))]
[RequireComponent(typeof(Flee))]
public class EnemyA : Enemy
{
    Seek seek;
    Flee flee;

    // Start is called before the first frame update
    void Start()
    {
        base.Create();

        seek = GetComponent<Seek>();
        seek.SetTarget(target);

        seek.enabled = false;

        flee = GetComponent<Flee>();
        flee.SetTarget(target);

        flee.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget = directionToTarget.normalized;

        float dotValue = -Vector3.Dot(target.forward, directionToTarget);

        //Inside viewing angle (flee)
        if(dotValue > 0.60f)
        {
            flee.enabled = true;
            seek.enabled = false;

            render.material.color = Color.white;
        }

        //Outside viewing angle (seek)
        else
        {
            flee.enabled = false;
            seek.enabled = true;

            render.material.color = Color.red;
        }

        if(transform.position.y < -20.0f)
        {
            parent.DeleteEnemy(gameObject);
        }
    }
}
