using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStat : CharacterStat 
{
	#region Menu Values
	public TMP_Text upgradePointText;
	public TMP_Text levelText;
	public TMP_Text scoreText;
	public TMP_Text healthText;
	public TMP_Text speedText;
	public TMP_Text strengthText;
	public TMP_Text resistanceText; 

	#endregion

	public Stat Level;
	public float upgradePointCounter;
    public float expCounter;
	public float expMaxCapacity = 100f;
	public float scoreCounter;
	public float levelUpScoreBonus = 10f;
	public float killCounter;

	public event System.Action<float, float> OnExpChanged;	

	PlayerShader playerDeathShader;
	FloatEffects playerFloatingEffect;

	public ParticleSystem levelUpEffect;
	public Image levelUpBorder;
	public Loader sceneLoaderReference;

	GameObject objectPoolerReference;

	void OnEnable()
	{		
		Health.IncreaseStat(40);
		Speed.IncreaseStat(3);
		Strength.IncreaseStat(5);
		Resistance.IncreaseStat(0);

		Level.IncreaseStat(0);
		upgradePointCounter = 0;
		expCounter = 0;
		scoreCounter = 0;
		killCounter = 0;

		currentHealth = Health.GetValue();

		playerDeathShader = transform.GetChild(1).GetComponent<PlayerShader>();
		playerFloatingEffect = transform.GetChild(1).GetComponent<FloatEffects>();

		upgradePointText.text = upgradePointCounter.ToString();
		levelText.text = Level.GetValue().ToString();
		scoreText.text = scoreCounter.ToString();
		healthText.text = Health.GetValue().ToString();
		speedText.text = Speed.GetValue().ToString();
		strengthText.text = Strength.GetValue().ToString();
		resistanceText.text = Resistance.GetValue().ToString();

		levelUpBorder.color = Color.grey;

		objectPoolerReference = GetComponent<ActivatePlayer>().activateObjectManager;
	}

	public void GainExp(float expAmount)
	{
		if (currentHealth > 0)
		{
			float excessExp = 0;

			if ((expCounter + expAmount) < expMaxCapacity)
			{
				expCounter += expAmount;			
			}
			
			else
			{
				Level.IncreaseStat(1);
				levelText.text = Level.GetValue().ToString();
				upgradePointCounter++;
				upgradePointText.text = upgradePointCounter.ToString();
				levelUpBorder.color = Color.yellow;

				levelUpEffect.Emit(10);

				IncreaseScore(levelUpScoreBonus);

				float hpToAdd;
				hpToAdd = Health.GetValue() * 0.2f;
				GainHealth(hpToAdd);

				excessExp = (expCounter + expAmount) - expMaxCapacity;
				expCounter = excessExp;
			}		

			if (OnExpChanged != null)
			{
				OnExpChanged(expMaxCapacity, expCounter);

				if (currentHealth > 0)
				{
					ShowExpUiText(expAmount);
				}			
			}
		}
	}

	public void IncreaseHealth()
	{
		if (upgradePointCounter > 0)
		{
			Health.IncreaseStat(5);
			upgradePointCounter--;
			upgradePointText.text = upgradePointCounter.ToString();
			healthText.text = Health.GetValue().ToString();
		}	

		if (upgradePointCounter == 0)
		{
			levelUpBorder.color = Color.grey;
		}	
	}

	public void IncreaseSpeed()
	{
		if (upgradePointCounter > 0 && Speed.GetValue() < 15)
		{
			Speed.IncreaseStat(0.5f);
			upgradePointCounter--;
			upgradePointText.text = upgradePointCounter.ToString();
			speedText.text = Speed.GetValue().ToString();
		}

		if (upgradePointCounter == 0)
		{
			levelUpBorder.color = Color.grey;
		}
	}

	public void IncreaseStrength()
	{
		if (upgradePointCounter > 0)
		{
			Strength.IncreaseStat(3);
			upgradePointCounter--;
			upgradePointText.text = upgradePointCounter.ToString();
			strengthText.text = Strength.GetValue().ToString();
		}

		if (upgradePointCounter == 0)
		{
			levelUpBorder.color = Color.grey;
		}
	}

	public void IncreaseResistance()
	{
		if (upgradePointCounter > 0)
		{
			Resistance.IncreaseStat(3);
			upgradePointCounter--;
			upgradePointText.text = upgradePointCounter.ToString();
			resistanceText.text = Resistance.GetValue().ToString();
		}

		if (upgradePointCounter == 0)
		{
			levelUpBorder.color = Color.grey;
		}
	}

	public void IncreaseKillCount()
	{
		killCounter++;
	}

	public void IncreaseScore(float scoreAmount)
	{
		scoreCounter += scoreAmount;
		scoreText.text = scoreCounter.ToString();
	}

	void ShowExpUiText(float _expAmount)
	{
		var go = ObjectPooler.Instance.SpawnFromPool("ExpUiText", transform.position, Quaternion.identity);
		go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _expAmount.ToString();		
	}

	public void HpShardCollected()
	{
		float hpToAdd;
		hpToAdd = Health.GetValue() * 0.2f;
		GainHealth(hpToAdd);
	}

	public void ExpShardCollected()
	{
		float expToAdd = 10f;
		GainExp(expToAdd);
	}

	public override void InitiateDeath()
	{
		GetComponent<PlayerController>().enabled = false;
		GetComponent<ActivatePlayer>().optionButton.interactable = false;
		GetComponent<ActivatePlayer>().actionButton.interactable = false;
		playerFloatingEffect.enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().isTrigger = true;
		AlertEnemiesOnDeath();
		playerDeathShader.ExecuteDeathSequence();
		StartCoroutine(LoadDeathMenu());

		PlayerPrefs.SetFloat("FinalKills", killCounter);
		PlayerPrefs.SetFloat("FinalLevel", Level.GetValue());
		PlayerPrefs.SetFloat("FinalScore", scoreCounter);
	}

	void AlertEnemiesOnDeath()
	{
		for (int i = 0; i < 15; i++)
        {
            if (objectPoolerReference.transform.childCount > 0)
            {
                if (objectPoolerReference.transform.GetChild(0).GetChild(i).gameObject.activeInHierarchy)
                {
                    objectPoolerReference.transform.GetChild(0).GetChild(i).GetComponent<EnemyController>().PlayerIsDead();
                }
            }            
        }
	}

	IEnumerator LoadDeathMenu()
	{
		yield return new WaitForSeconds(6f);
		sceneLoaderReference.LoadDeathScreen();
	}
}
