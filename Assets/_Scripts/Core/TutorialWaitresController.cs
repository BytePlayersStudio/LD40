using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaitresController : MonoBehaviour {

    public GameObject buble;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            buble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            buble.SetActive(false);
        }
    }
}
