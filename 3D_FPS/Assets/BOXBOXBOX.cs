using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOXBOXBOX : MonoBehaviour
{
    public void sdf(int value)
    {
        if (value <= 0)
            GetComponent<Renderer>().material.color = Color.red;
    }
}
