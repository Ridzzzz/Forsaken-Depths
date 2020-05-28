using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStat : MonoBehaviour 
{
	public float currentHealth {get;set;}

	public Stat Health;
	public Stat Speed;
	public Stat Strength;
	public Stat Resistance;

	public event System.Action<Stat, float> OnHealthChanged;

	public void TakeDamage(float damageAmount, float characterType)
	{
		if (currentHealth > 0)
		{
			damageAmount -= Resistance.GetValue();
			damageAmount = Mathf.Clamp(damageAmount, 1, float.MaxValue);

			currentHealth -= damageAmount;		

			if (OnHealthChanged != null)
			{
				OnHealthChanged(Health, currentHealth);			
				ShowHealthDamageUiText(damageAmount, characterType);		
			}
		}

		if (currentHealth <= 0)
		{
			InitiateDeath();
		}
	}

	public void GainHealth(float healthAmount)
	{
		if (currentHealth > 0)
		{
			if (currentHealth < Health.GetValue())
			{
				currentHealth = Mathf.Clamp(currentHealth + healthAmount, 0, Health.GetValue());
			}		

			if (OnHealthChanged != null)
			{
				OnHealthChanged(Health, currentHealth);
				ShowHealthHealingUiText(healthAmount);		
			}
		}
	}

	void ShowHealthDamageUiText(float _damageAmount, float _characterType)
	{	
		if (_characterType == 0)
		{
			var go = ObjectPooler.Instance.SpawnFromPool("DamageUiText", transform.position, Quaternion.identity);
			go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _damageAmount.ToString();	
		}

		else
		{
			var go = ObjectPooler.Instance.SpawnFromPool("DamageUiText", transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
			go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _damageAmount.ToString();	
		}
			
	}

	void ShowHealthHealingUiText(float _healthAmount)
	{
		var go = ObjectPooler.Instance.SpawnFromPool("HealingUiText", transform.position, Quaternion.identity);
		go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _healthAmount.ToString();		
	}

	public virtual void InitiateDeath(){}
}
