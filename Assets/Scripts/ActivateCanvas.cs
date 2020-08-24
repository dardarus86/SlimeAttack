using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.gameObject.SetActive(true);
        Debug.Log("activated");
    }
}
