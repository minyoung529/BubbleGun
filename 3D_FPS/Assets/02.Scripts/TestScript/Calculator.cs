using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    public delegate float Calculate(float a, float b);
    Calculate onCalculate;
    Action action;

    private void Start()
    {
        onCalculate = Sum;
        onCalculate += Substract;
        onCalculate += Multiply;
        onCalculate -= Multiply;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onCalculate.Invoke(1, 10);
        }
    }

    public float Sum(float a, float b)
    {
        Debug.Log($"Sum : {a + b}");
        return a + b;
    }
    public float Substract(float a, float b)
    {
        Debug.Log($"Substract : {a - b}");
        return a - b;
    }
    public float Multiply(float a, float b)
    {
        Debug.Log($"Multiply : {a * b}");
        return a * b;
    }
}
