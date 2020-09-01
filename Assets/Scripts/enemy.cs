using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{

    //variables

    Vector2 jumpvelocity;
    [SerializeField] float jumpVelocityX = 1f;
    [SerializeField] float jumpVelocityY = 1f;
    [SerializeField] AudioClip splatClip;
    public int enemyHealth;
    int enemyStartingHealth;
    [SerializeField] float enemyMass = 2;
    [SerializeField] GameObject enemyMediumPreFab;
    [SerializeField] GameObject enemySmallPreFab;
    [SerializeField] Vector2 spawnLeft = new Vector2(-0.0f, 0.0f);
    [SerializeField] Vector2 spawnRight = new Vector2(0.0f, 0.0f);
    //states
    [SerializeField] bool isJumping = false;
    bool hasSpawned = false;
    bool deathanim;

    //cached References
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBody;
    BoxCollider2D myFeet;
    Animator slimeAnimation;
    GameSession myGameSession;
    GameObject remainingEnemies;
    public Slider healthBar;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBody = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        slimeAnimation = GetComponent<Animator>();
        myGameSession = FindObjectOfType<GameSession>();
        remainingEnemies = GameObject.Find("Enemies");
    }
    void FixedUpdate()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            //myRigidBody.velocity = Vector2.zero;
            myRigidBody.angularVelocity = 0;
            Jump();
        }
        CheckBulletCollision();
    }

    private void Jump()
    {
        if (myRigidBody.velocity.x > Mathf.Epsilon)
        {
            myRigidBody.velocity = Vector2.zero;
            jumpvelocity = new Vector2(Random.Range(40,100)* enemyMass, Random.Range(180, 240) * enemyMass);
        }
        else
        {
            myRigidBody.velocity = Vector2.zero;
            jumpvelocity = new Vector2(-(Random.Range(40, 100) * enemyMass), Random.Range(160, 200) * enemyMass);
        }

        AudioSource.PlayClipAtPoint(splatClip, Camera.main.transform.position);
        slimeAnimation.SetTrigger("Jump");
        myRigidBody.AddForce(jumpvelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private void CheckBulletCollision()
    {
        if(myBody.IsTouchingLayers(LayerMask.GetMask("Bullets", "Player")))
        {
            if (enemyHealth <= 0)
            {
                
                Die();
            }
            else if (enemyHealth > 0)
            {
                enemyHealth--;
            }
        }
        healthBar.value = enemyHealth;
    }

    private void Die()
    {
        myBody.enabled = !myBody.enabled;
        myFeet.enabled = !myFeet.enabled;
        switch(name)
        {
            case "Large Enemy":
                DeathAnim();
               
                SpawnChildren(enemyMediumPreFab);
                myGameSession.AddScore(100);
                hasSpawned = true;

                break;
            case "Medium Enemy":
                DeathAnim();
                
                SpawnChildren(enemySmallPreFab);
                myGameSession.AddScore(50);
                hasSpawned = true;
                break;
            case "Small Enemy":
                DeathAnim();
                myGameSession.AddScore(10);
                break;
        }
    }

    private void DeathAnim()
    {
        if(!deathanim)
        {
            slimeAnimation.SetTrigger("Dead");
            Destroy(gameObject,0.7f);
            deathanim = true;
        }
    }

    private void SpawnChildren(GameObject enemyprebsize)
    {
       
        if (!hasSpawned)
        {
            GameObject child1 = Instantiate(enemyprebsize, transform.position, Quaternion.identity);
            child1.name = enemyprebsize.name;
            child1.transform.SetParent(remainingEnemies.transform);
            child1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            child1.GetComponent<Rigidbody2D>().angularVelocity = 0;
            Vector2 spawnLeft = new Vector2(-2.0f, 3.0f);
            child1.GetComponent<Rigidbody2D>().AddForce(spawnLeft, ForceMode2D.Impulse);
            GameObject child2 = Instantiate(enemyprebsize, transform.position, Quaternion.identity);
            child2.name = enemyprebsize.name;
            child2.transform.SetParent(remainingEnemies.transform);
            child2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            child2.GetComponent<Rigidbody2D>().angularVelocity = 0;
            Vector2 spawnRight = new Vector2(2.0f, 3.0f);
            child2.GetComponent<Rigidbody2D>().AddForce(spawnRight, ForceMode2D.Impulse);
            
        }
    }


}
