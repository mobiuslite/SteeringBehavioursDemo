using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pursue))]
[RequireComponent(typeof(Evade))]
public class EnemyB : Enemy
{
    Pursue pursue;
    Evade evade;

    [SerializeField]
    float distanceToDodge = 5.0f;

    private void Start()
    {
        base.Create();

        pursue = GetComponent<Pursue>();
        pursue.SetTarget(target);

        pursue.enabled = true;

        evade = GetComponent<Evade>();
        evade.SetTarget(target);

        evade.enabled = true;
    }

    private void Update()
    {
        Transform closestBullet = null;
        float distanceOfClosestBullet = float.MaxValue;

        //Find closest bullet
        foreach(Transform bullet in target.GetComponent<PlayerMovement>().GetBulletsTransform())
        {
            float tempDistance = Vector3.Distance(bullet.position, transform.position);
            if (tempDistance < distanceOfClosestBullet)
            {
                closestBullet = bullet;
                distanceOfClosestBullet = tempDistance;
            }
        }

        //EVADING
        //Bullet is in dodging range, now see how close it is to the enemy.
        if(distanceOfClosestBullet < distanceToDodge)
        {
            //Inverts the ratio, so that the closer it is, the bigger the ratio.
            //float ratio = distanceOfClosestBullet / distanceToDodge;
            //float invsRatio = Mathf.Abs(ratio - 1.0f);

            evade.ChangeRatio(1.0f);
            evade.SetTarget(closestBullet);
            evade.evade = true;

            pursue.ChangeRatio(0.0f);

            render.material.color = new Color(0.0f, 0.0f, 0.0f);
        }

        //PURSUING
        else
        {
            evade.evade = false;

            pursue.ChangeRatio(1.0f);
            evade.ChangeRatio(0.0f);

            //Purple
            render.material.color = new Color(1.0f, 0.0f, 1.0f);
        }

        if (transform.position.y < -20.0f)
        {
            parent.DeleteEnemy(gameObject);
        }
    }
}
