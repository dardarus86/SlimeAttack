using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    //variables

    Vector2 jumpvelocity;
    [SerializeField] float jumpVelocityX = 1f;
    [SerializeField] float jumpVelocityY = 1f;
    [SerializeField] AudioClip splatClip;
    [SerializeField] int enemyHealth;
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

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBody = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        slimeAnimation = GetComponent<Animator>();
        
    }
    void FixedUpdate()
    {
        if (myRigidBody.velocity.x > Mathf.Epsilon)
        {
            jumpvelocity = new Vector2(jumpVelocityX, jumpVelocityY);
        }
        else
        {
            jumpvelocity = new Vector2(-jumpVelocityX, jumpVelocityY);
        }

        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidBody.velocity = Vector2.zero;
            myRigidBody.angularVelocity = 0;
            Jump();
        }

        CheckBulletCollision();
    }

    private void Jump()
    {
        AudioSource.PlayClipAtPoint(splatClip, Camera.main.transform.position);
        slimeAnimation.SetTrigger("Jump");
        myRigidBody.AddForce(jumpvelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private void CheckBulletCollision()
    {
        if(myFeet.IsTouchingLayers(LayerMask.GetMask("Bullets")))
        {
            Debug.Log("ive been hit");
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if(enemyHealth>0)
        {
            enemyHealth--;
        }
        if (enemyHealth == 0)
        {
            
            Die();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        switch(name)
        {
            case "Large Enemy":
                DeathAnim();
                Debug.Log("Spawn medium enemies");
                SpawnChildren(enemyMediumPreFab);
                hasSpawned = true;

                break;
            case "Medium Enemy(Clone)":
                DeathAnim();
                Debug.Log("Spawn small enemies");
                SpawnChildren(enemySmallPreFab);
                hasSpawned = true;
                break;
            case "Small Enemy(Clone)":
                DeathAnim();
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
            child1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            child1.GetComponent<Rigidbody2D>().angularVelocity = 0;
            Vector2 spawnLeft = new Vector2(-2.0f, 3.0f);
            child1.GetComponent<Rigidbody2D>().AddForce(spawnLeft, ForceMode2D.Impulse);
            GameObject child2 = Instantiate(enemyprebsize, transform.position, Quaternion.identity);
            child2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            child2.GetComponent<Rigidbody2D>().angularVelocity = 0;
            Vector2 spawnRight = new Vector2(2.0f, 3.0f);
            child2.GetComponent<Rigidbody2D>().AddForce(spawnRight, ForceMode2D.Impulse);
            
        }
    }


}
