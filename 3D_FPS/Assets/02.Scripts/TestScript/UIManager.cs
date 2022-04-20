using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text stateText;

    public void OnPlayerDead()
    {
        stateText.text = "¿ì¾î¤Ã¤Ã¾î";
    }
}
