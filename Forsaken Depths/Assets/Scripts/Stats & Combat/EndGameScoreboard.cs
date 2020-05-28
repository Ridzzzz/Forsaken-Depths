using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameScoreboard : MonoBehaviour
{
    public TMP_Text endGameKills;
    public TMP_Text endGameLevel;
    public TMP_Text endGameScore;

    void OnEnable()
    {
        endGameKills.text = PlayerPrefs.GetFloat("FinalKills").ToString();
        endGameLevel.text = PlayerPrefs.GetFloat("FinalLevel").ToString();
        endGameScore.text = PlayerPrefs.GetFloat("FinalScore").ToString();
    }
}
