using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] float DeathTimer = 1f;

    [SerializeField] float horizontalMove;
    [SerializeField] float verticalMove;

    [SerializeField] float invulTimeAfterGettingHit = 2f;
    [SerializeField] float invulTimeNextHit = 0.0f;
    [SerializeField] float playerDeathSloMoFactor = 0.9f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioClip bulletFireSound;
    [SerializeField] Vector3 bulletSpawnToPlayerOffset = new Vector3(0, 5f,0);
    [SerializeField] GameObject bulletSpawnLocation;
    float gravityScaleAtStart;
    public VariableJoystick joystick;
    public JoyButton button;
   
    //state
    bool isAlive = true;
    [SerializeField] bool canShoot = true;
    [SerializeField] bool canRun = false;

    //cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2d;
    BoxCollider2D myFeet;
    public GameSession GameSession;
    public LevelLoader levelLoader;




    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider2d = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        GameSession = FindObjectOfType<GameSession>();
        levelLoader = FindObjectOfType<LevelLoader>();
        joystick = FindObjectOfType<VariableJoystick>();
        button = FindObjectOfType<JoyButton>();
       
        gravityScaleAtStart = myRigidBody.gravityScale;

    }

     void Update()
    {
        if (!isAlive) { return; }
            
    if (canRun == false){Run();}
  
        if (canShoot == false){Shoot();}
       Climb();

       FlipSprite();
       TakeDamage();



        NextLevelCollisionCheck();

    }


    private void Run()
    {

        if(joystick.Horizontal >= .6f)
        {
            horizontalMove = runSpeed;
        }
        else if(joystick.Horizontal <= -.6f)
        {
            horizontalMove = runSpeed;
        }
        else
        {
            horizontalMove = 0;
        }
     
        Vector2 direction = new Vector2(joystick.Horizontal* horizontalMove, myRigidBody.velocity.y);
      
        myRigidBody.velocity = direction;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //value is between -1 and +1
        //Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        //myRigidBody.velocity = playerVelocity;

        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        //myAnimator.SetBool("Running", playerHasHorizontalSpeed);

        if      (joystick.Horizontal == 0){canShoot = false;}
        else if (joystick.Horizontal != 0){canShoot = true;}
    }


    private void NextLevelCollisionCheck()
    {
       if( myBodyCollider2d.IsTouchingLayers(LayerMask.GetMask("LevelTransition")))
        {
            levelLoader.LoadNextLevel();
        }
    }


    private void Climb()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            if (joystick.Vertical >= .6f)
            {
                verticalMove = runSpeed;
            }
            else if (joystick.Vertical <= -.6f)
            {
                verticalMove = runSpeed;
            }
            else
            {
                verticalMove = 0;
            }
            
            Vector2 direction = new Vector2(joystick.Horizontal * horizontalMove, joystick.Vertical* verticalMove);
         
            myRigidBody.velocity = direction;

            //float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            //Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            //myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
        }
        else if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
    }

    private void Shoot()
    {
        if (button.pressed)
        {
            canRun = true;
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                myAnimator.SetBool("isFiring", true);
                BulletFire();
            }
        }
        else if (!button.pressed)
        {
            myAnimator.SetBool("isFiring", false);
            canRun = false;
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

    private void TakeDamage()
    {
        if(myBodyCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if (Time.time > invulTimeNextHit)
            {
                invulTimeNextHit = Time.time + invulTimeAfterGettingHit;
                myAnimator.SetTrigger("Hit");
                if (GameSession.GetPlayerLife() > 0)
                {
                    GameSession.Removelife();
                }
                else if (GameSession.GetPlayerLife() <= 0)
                {
                    StartCoroutine(Death());
                }
            }
        }
    }

    private IEnumerator Death()
    {
        gameObject.layer = 14;
        myAnimator.SetTrigger("Dead");
        
        myRigidBody.AddForce(new Vector2(0.5f, 70f),ForceMode2D.Impulse);
        
        //Time.timeScale = playerDeathSloMoFactor;
        yield return new WaitForSeconds(DeathTimer);
       // Time.timeScale = 1f;
        SceneManager.LoadScene("Game Over");
        //Destroy(gameObject);
        //show end screen with total score
    }
}



//features that might be added

//private void Jump()
//{
//    if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

//    if (CrossPlatformInputManager.GetButtonDown("Jump"))
//    {
//        Vector2 playerJumpVelocity = new Vector2(0f, jumpSpeed);
//        myRigidBody.velocity += playerJumpVelocity;
//    }
//}