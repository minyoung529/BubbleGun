using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ScoreManager>();
            if (instance == null)
                instance = new GameObject("ScoreManager").AddComponent<ScoreManager>();

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private int score = 0;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }
}
