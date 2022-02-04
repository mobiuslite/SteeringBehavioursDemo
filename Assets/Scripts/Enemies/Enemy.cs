using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform target;
    protected SpawnManager parent;

    protected Renderer render;

    protected void Create()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<Renderer>();
    }

    public virtual void Delete()
    {
        if(parent != null)
            parent.DeleteEnemy(gameObject);
    }

    public void SetParent(SpawnManager spawnManager)
    {
        parent = spawnManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit player!");
            if (parent != null)
                parent.HitPlayer();
        }
    }
}
