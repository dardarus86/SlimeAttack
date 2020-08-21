using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraRetargetNewBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");

        var confiner = virtualCamera.GetComponent<CinemachineConfiner>();

        confiner.InvalidatePathCache();
        confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Box").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
