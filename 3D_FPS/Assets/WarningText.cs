using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour
{
    public void NewMethod(int value)
    {
        if (value <= 0)
            GetComponent<Text>().color = Color.red;

        else if (value <= 50)
            GetComponent<Text>().color = Color.blue;
    }
}
