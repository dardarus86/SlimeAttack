using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

   

    BoxCollider2D myBoxCollider;
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        
       if(myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Ground")))
        {
            gameObject.SetActive(false);
            Destroy(gameObject,0.1f);
        }

    }

}
