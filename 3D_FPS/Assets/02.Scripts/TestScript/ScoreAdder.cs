using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    private Material cubeMaterial;

    private void Start()
    {
        cubeMaterial = GetComponent<Renderer>().material;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ScoreManager.Instance.AddScore(10);
        }
        if(ScoreManager.Instance.Score >= 100)
        {
            cubeMaterial.color = Color.red;
        }
    }
}
