using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    
    private int _EnemyTime;
    private int _fatnessLvl1;
    private int _fatnessLvl2;

    public int enemyContactTime;
    public int currentFatPoints;
    public int maxFatPoints;

    [HideInInspector]
    public int fatness;

    #endregion

    #region Unity Functions

    void Start()
    {
        _EnemyTime = 0;

        fatness = 0;

        _fatnessLvl1 = maxFatPoints / 3;
        _fatnessLvl2 = _fatnessLvl1 * 2;
    }

    void Update()
    {
        // If the player is alive...
        if (IsAlive())
        {
            if (currentFatPoints >= _fatnessLvl2)
            {
                // The player reachs Lvl 1 of fatness
                fatness = 2;
            }
            else if(currentFatPoints >= _fatnessLvl1)
            {
                // The player reachs Lvl 2 of fatness
                fatness = 1;
            }
        }
        // If not...
        else
        {
            // GAME OVER MENU
        }
    }

    #endregion

    #region Custom Functions

    // If currentFatPoints is higher than the maximum, is Dead
    private bool IsAlive()
    {
        if (currentFatPoints >= maxFatPoints)
            return false;
        return true;
    }

    // If the player touches the trigger of an enemy, will gain Fatness
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
