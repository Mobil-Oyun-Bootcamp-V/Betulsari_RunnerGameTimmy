using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 offset;
    Transform player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        offset = player.position-transform.position;
    }
    
    void LateUpdate()
    {
        transform.position = player.position - offset;
    }
}
