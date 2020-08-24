using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbar : MonoBehaviour
{

    public enemy enemyHealth;
    int enemyStartingHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = this.transform.parent.GetComponent<enemy>();
        enemyStartingHealth = enemyHealth.enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void ChangeHealthBar()
    {
        Vector3 healthChange = new Vector3(enemyStartingHealth/enemyHealth.enemyHealth, 1, 1);
        transform.localScale += healthChange;
    }
}
