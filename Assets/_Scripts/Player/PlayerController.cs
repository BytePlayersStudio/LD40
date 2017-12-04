using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    
    private int _EnemyTime;
    private int _fatnessLvl1;
    private int _fatnessLvl2;
	private int _fatnessLvl0;

    public int currentCriticP;
    public int currentFatPoints;

    public int enemyContactTime;
    public int maxFatPoints;
    public int maxCriticPercentage;
	public int foodFatnessIncrease;

    public AudioClip eating;
    public GameObject gameOverMenu;

    [HideInInspector]
    public int fatness;
    [HideInInspector]
    public bool isCriticReady;

    public BarController bc;

    #endregion

    #region Unity Functions

    void Start()
    {
        _EnemyTime = 0;

        fatness = 0;

		_fatnessLvl0 = 0;
        _fatnessLvl1 = maxFatPoints / 3;
        _fatnessLvl2 = _fatnessLvl1 * 2;

        bc.fatMax = maxFatPoints;
        bc.critMax = maxCriticPercentage;

        isCriticReady = false;
		if (foodFatnessIncrease == 0) foodFatnessIncrease = 10;

        StartCoroutine(addCriticPercentage());
    }

    void Update()
    {
        bc.setFatValue(currentFatPoints);
        bc.setCritValue(currentCriticP);
        // If the player is alive...
        if (IsAlive())
        {
			if (currentFatPoints >= _fatnessLvl2)
			{
				// The player reachs Lvl 1 of fatness
				fatness = 2;
			}
			else if (currentFatPoints >= _fatnessLvl1)
			{
				// The player reachs Lvl 2 of fatness
				fatness = 1;
			}
			else
			{
				fatness = 0;
			}
        }
        // If not...
        else
        {
            // GAME OVER MENU
            GameOver();
        }
    }

    #endregion

    #region Custom Functions

    // If currentFatPoints is higher than maxFatPoints, is Dead
    private bool IsAlive()
    {
        if (currentFatPoints >= maxFatPoints)
            return false;
        return true;
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    // If the player touches the trigger of an enemy, will gain Fatness
    private void OnTriggerStay2D(Collider2D col)
    {
        ++_EnemyTime;

        if (col.gameObject.tag == "Enemy" && _EnemyTime >= enemyContactTime)
        {
            ++currentFatPoints;
            _EnemyTime = 0;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			currentFatPoints += foodFatnessIncrease;
            GetComponent<AudioSource>().clip = eating;
            GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
		}
	}
	#endregion

	#region Coroutines
	// This courutine runs always, increasing the value of "Critic Percentage"
	IEnumerator addCriticPercentage()
    {
        while (true)
        {
            if (currentCriticP < maxCriticPercentage)
            {
                currentCriticP += 5;
                if (isCriticReady)
                    isCriticReady = false;
                yield return new WaitForSeconds(1);
            }
            else
            {
                if (!isCriticReady)
                    isCriticReady = true;
                yield return null;
            }
        }
    }

    #endregion
}
