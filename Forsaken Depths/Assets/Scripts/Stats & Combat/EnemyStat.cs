using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStat 
{
	float baseHealth = 20f;
	float baseSpeed = 2.5f;
	float baseStrength = 3f;
	float baseResistance = 0f;

	EnemyController enemyController;
	PlayerStat getPlayerStat;
	float getPlayerLevel;

	EnemyShader enemyDeathShader;
	WeaponShader weaponDeathShader;

	void OnEnable() 
	{
		enemyController = GetComponent<EnemyController>();
		getPlayerStat = enemyController.target.GetComponent<PlayerStat>();
		getPlayerLevel = getPlayerStat.Level.GetValue();

		ScaleStats(getPlayerLevel);	

		enemyDeathShader = transform.GetChild(1).GetComponent<EnemyShader>();
		weaponDeathShader = transform.GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<WeaponShader>();
	}

	void ScaleStats(float playerLevel)
	{
		if (playerLevel == 0)
		{
			Health.CopyStatValue(baseHealth);
			Speed.CopyStatValue(baseSpeed);
			Strength.CopyStatValue(baseStrength);
			Resistance.CopyStatValue(baseResistance);

			currentHealth = Health.GetValue();
		}

		else
		{
			float modifiedHealth = baseHealth + (playerLevel*0.5f) + playerLevel;
			float modifiedSpeed = Mathf.Clamp(baseSpeed + (playerLevel*0.25f), baseSpeed, 15);
			float modifiedStrength = baseStrength + (playerLevel*0.5f) + playerLevel;
			float modifiedResistance = baseResistance + (playerLevel*0.5f) + playerLevel;

			Health.CopyStatValue(modifiedHealth);
			Speed.CopyStatValue(modifiedSpeed);
			Strength.CopyStatValue(modifiedStrength);
			Resistance.CopyStatValue(modifiedResistance);

			currentHealth = Health.GetValue();
		}
	}

	public override void InitiateDeath()
	{
		float enemyDeathExp = 20f;
		float enemyDeathScoreBonus = 5f;

		enemyController.enemyIsDead = true;
		getPlayerStat.GainExp(enemyDeathExp);
		getPlayerStat.IncreaseScore(enemyDeathScoreBonus);
		getPlayerStat.IncreaseKillCount();

		enemyDeathShader.ExecuteDeathSequence();
		weaponDeathShader.ExecuteDeathSequence();
	}
}
