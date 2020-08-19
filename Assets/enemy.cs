using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    //variables

    Vector2 jumpvelocity;
    [SerializeField] float jumpVelocityX = 1f;
    [SerializeField] float jumpVelocityY = 1f;


    //states
    [SerializeField] bool isJumping = false;

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
    }

    private void Jump()
    {
        slimeAnimation.SetTrigger("Jump");
        myRigidBody.AddForce(jumpvelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
