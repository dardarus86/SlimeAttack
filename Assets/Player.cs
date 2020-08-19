using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float projectileSpeed = 500f;
    [SerializeField] float fireRate = 1.0f;
    [SerializeField] float nextFire = 0.0f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioClip bulletFireSound;
    [SerializeField] Vector3 bulletSpawnToPlayerOffset = new Vector3(0, 5f,0);
    [SerializeField] GameObject bulletSpawnLocation;



    //state
    bool isAlive = true;

    //cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2d;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider2d = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        Jump();
        FlipSprite();
        Climb();
        Shoot();
    }


    private void Run()
    {
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 playerJumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += playerJumpVelocity;
        }
    }

    private void Climb()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

    }

    private void Shoot()
    {
        if(CrossPlatformInputManager.GetButton("Fire1"))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                myAnimator.SetBool("isFiring", true);
                BulletFire();
            }
        }
        else
        {
            myAnimator.SetBool("isFiring", false);
        }
    }

    private void BulletFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * projectileSpeed * Time.deltaTime;
        AudioSource.PlayClipAtPoint(bulletFireSound, transform.position);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
