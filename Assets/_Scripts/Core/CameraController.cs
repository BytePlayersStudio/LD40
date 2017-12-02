using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Variables
    // Diference between the margin of the camera, at spawn time, and the player
    private Vector3 _offset;
    public GameObject player;
    #endregion

    #region Unity Functions
    void Start()
    {
        _offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + _offset.x,0, transform.position.z);
    }

    #endregion
}
