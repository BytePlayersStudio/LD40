using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables

    private bool _eating;
    private int _EnemyTime;

    public int enemyContactTime;
    public float currentFatPoints;
    public float maxFatPoints;

    #endregion

    #region Unity Functions

    void Start()
    {
        _eating = false;
        _EnemyTime = 0;
    }

    void Update()
    {
        // If life is higher than 0...
        if (IsAlive())
        {

        }
    }

    #endregion

    #region Custom Functions

    private bool IsAlive()
    {
        if (currentFatPoints >= maxFatPoints)
            return false;
        return true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        ++_EnemyTime;
        Debug.Log("Choca");

        if (col.gameObject.tag == "Enemy" && _EnemyTime == enemyContactTime)
        {
            ++currentFatPoints;
            _EnemyTime = 0;
        }
    }

    #endregion

}
