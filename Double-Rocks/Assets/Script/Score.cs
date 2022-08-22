using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;

    public static Score instance;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    public void AddPoint(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }
}
