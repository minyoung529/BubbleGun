using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{

    Func<int> func;
    Func<int, int> func2;

    public delegate void Send(string reciver);
    event Send onSend;

    private void Awake()
    {
        func += () => 3 + 5;
        func += () => 7 + 4;
        func += () => 1 + 3;

        int a = func.Invoke();
        //func.in
        Debug.Log(a);
    }

    void SendMail(string reciver)
    {
        Debug.Log("Mail send to : " + reciver);
    }

    void sdfsdfs()
    {

    }
    void SendMoney(string reciver)
    {
        Debug.Log("Money send to : " + reciver);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onSend.Invoke("GGM");
        }
    }
}

