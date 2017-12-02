using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 _offset;

    public GameObject player;
    
    void Start()
    {
        _offset = transform.position - player.transform.position;
    }

    void Update()
    {
        transform.position = player.transform.position + _offset;
    }
}
