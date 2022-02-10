using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float timeAlive = 5.0f;
    float elapsedTime = 0.0f;

    PlayerMovement playerParent = null;
    EnemyC enemyParent = null;
   
    // Update is called once per frame
    void Update()
    {
        if(elapsedTime >= timeAlive)
        {
            if (playerParent != null)
                playerParent.DeleteBullet(this);

            if (enemyParent != null)
                enemyParent.DeleteBullet(this);
        }

        elapsedTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Hit enemy/player
        if (this.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            bool hitEnemy = false;
            if (collision.gameObject.layer != LayerMask.NameToLayer("Environment") && collision.gameObject.layer != LayerMask.NameToLayer("EnemyBullet"))
            {
                collision.gameObject.GetComponent<Enemy>().Delete();
                hitEnemy = true;
            }
            if (playerParent != null)
                playerParent.DeleteBullet(this, hitEnemy);
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                enemyParent.BulletHitPlayer();
            }
            if (enemyParent != null)
                enemyParent.DeleteBullet(this);
        }       
    }

    public void SetParent(PlayerMovement p)
    {
        if (enemyParent != null)
            Debug.LogError("Enemy parent already set in bullet! Something isn't right here....");

        playerParent = p;
    }

    public void SetParent(EnemyC p)
    {
        if (playerParent != null)
            Debug.LogError("Player parent already set in bullet! Something isn't right here....");

        enemyParent = p;
    }
}
