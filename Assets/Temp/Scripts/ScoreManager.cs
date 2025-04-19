using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score;

    public Text scoreText;

    public void ScoreIncrease()
    {
        score++;
        scoreText.text = "Score£º" + score;
    }
}
