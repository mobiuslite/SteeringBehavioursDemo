using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float movementAmount;
    [SerializeField]
    Text killText;
    [SerializeField]
    Text highscoreText;

    [Space(10)]
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float bulletSpeed;

    [Space(10)]
    [SerializeField]
    Light flashLight;
    [SerializeField]
    float lightFlashLength = 0.025f;
    float flashLightElapsed = 0.0f;

    Rigidbody rb;

    List<Bullet> bulletsFired;

    int kills = 0;
    int curHighscore = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletsFired = new List<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashLight.enabled)
        {
            if(flashLightElapsed >= lightFlashLength)
            {
                flashLight.enabled = false;
                flashLightElapsed = 0.0f;
            }
            else
            {
                flashLightElapsed += Time.deltaTime;
            }
        }

        //Player rotation
        Ray rayCasting = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.down, Vector3.zero);
        float rayLength;

        Vector3 dirFacing = Vector3.zero;

        if (ground.Raycast(rayCasting, out rayLength))
        {
            Vector3 mouse = rayCasting.GetPoint(rayLength);
            mouse.y = transform.position.y; 
            transform.LookAt(mouse);

            dirFacing = mouse - transform.position;
            dirFacing.y = 0;

            dirFacing = dirFacing.normalized;
        }

        //Player movement
        Vector3 movementDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementDir += Vector3.forward;
            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementDir += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementDir += Vector3.left;
        }  
        else if (Input.GetKey(KeyCode.D))
        {
            movementDir += Vector3.right;
        }

        rb.velocity = movementAmount * movementDir.normalized;

        //SHOOTING
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.layer = LayerMask.NameToLayer("PlayerBullet");

            newBullet.GetComponent<Bullet>().SetParent(this);
            bulletsFired.Add(newBullet.GetComponent<Bullet>());

            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            bulletRb.velocity = dirFacing * bulletSpeed;

            //Muzzle Flash
            flashLight.enabled = true;
        }

    }

    public List<Transform> GetBulletsTransform()
    {
        List<Transform> bulletsTransform = new List<Transform>();
        foreach(Bullet b in bulletsFired)
        {
            bulletsTransform.Add(b.transform);
        }

        return bulletsTransform;
    }

    public void DeleteBullet(Bullet b, bool hitEnemy = false)
    {
        bulletsFired.Remove(b);
        Destroy(b.gameObject);

        if (hitEnemy)
        {
            kills++;
            killText.text = "Kills: " + kills;
        }
    }

    public void ResetScore()
    {
        //Set highscore if current score is bigger
        if (kills > curHighscore)
        {
            curHighscore = kills;
            highscoreText.text = "Highscore: " + curHighscore;
        }
        
        kills = 0;
        killText.text = "Kills: " + kills;
    }
}
