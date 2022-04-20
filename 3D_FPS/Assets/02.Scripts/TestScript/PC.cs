using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public string playerName = "Sdf";
    public float hp = 100f;
    public float defense = 50f;
    public float damage = 30f;

    private delegate void Boost(PC pc);
    Boost playerBoost;

    private Booster booster;

    private void Awake()
    {
        booster = FindObjectOfType<Booster>();

        playerBoost += booster.ShieldBoost;
        playerBoost += booster.DamageBoost;
        playerBoost += booster.HealthBoost;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerBoost.Invoke(this);
        }
    }
}
