using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Approach))]
public class EnemyC : Enemy
{
    Approach approach;

    [SerializeField]
    float firingRate = 1.0f;
    [SerializeField]
    float firingSpeed = 30.0f;

    [Space(10)]
    [SerializeField]
    GameObject bulletPrefab;

    float elapsedFiringTime = 0.0f;
    List<Bullet> bulletsFired;

    void Start()
    {
        base.Create();

        approach = GetComponent<Approach>();
        approach.SetTarget(target);

        approach.enabled = true;

        bulletsFired = new List<Bullet>();

        render.material.color = Color.green;
    }

    void Update()
    {
        elapsedFiringTime += Time.deltaTime;
        if(elapsedFiringTime >= firingRate && approach.IsInFiringRange())
        {
            //Fire bullet!
            Vector3 dir = FindPlayerFuturePosition() - transform.position;
            dir.y = 0f;
            dir = dir.normalized;

            Vector3 velo = dir * firingSpeed;

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.layer = LayerMask.NameToLayer("EnemyBullet");

            newBullet.GetComponent<Bullet>().SetParent(this);
            bulletsFired.Add(newBullet.GetComponent<Bullet>());

            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            bulletRb.velocity = velo;

            //Makes the bullet red.
            newBullet.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.red * 10.0f);

            //Doing this because for some reason layer 8 and layer 11 (enemy, enemybullet) still want to collide with each other
            //dispite being disabled in the Physics collision matrix?????? (Yes it's disabled in the 3D physics matrix, not the 2D one)
            Collider bulletCollider = newBullet.GetComponent<Collider>();
            Collider thisCollider = GetComponent<Collider>();
            Physics.IgnoreCollision(bulletCollider, thisCollider, true);

            elapsedFiringTime = 0.0f;
        }

        if (approach.IsInFiringRange())
        {
            render.material.color = Color.blue;
        }
        else
        {
            render.material.color = Color.green;
        }

    }

    //Uses so that this enemy will fire at the place the player is GOING to be
    //not where they currently are.
    Vector3 FindPlayerFuturePosition()
    {
        //velocity is distance over time.
        //we already have velocity, so lets find time

        //v = d/t
        //v*t = d;
        //t = d/v;

        float time = approach.GetDistanceToTarget() / firingSpeed;

        Vector3 futurePosition = target.position + target.gameObject.GetComponent<Rigidbody>().velocity * time;
        return futurePosition;
    }

    public void BulletHitPlayer()
    {
        parent?.HitPlayer();
    }

    public void DeleteBullet(Bullet b)
    {
        bulletsFired.Remove(b);
        Destroy(b.gameObject);
    }

    public override void Delete()
    {
        foreach(Bullet b in bulletsFired)
        {
            Destroy(b.gameObject);
        }
        bulletsFired.Clear();

        parent.DeleteEnemy(gameObject);
    }
}
