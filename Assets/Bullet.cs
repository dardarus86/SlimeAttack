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
    void Update()
    {
       
        
       if(myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Debug.Log("collided layer");
            Destroy(gameObject,0.02f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided on collision");
        Destroy(gameObject);
    }
}
