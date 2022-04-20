using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public void HealthBoost(PC target)
    {
        Debug.Log(target.playerName + "의 체력을 강화했다!!");
        target.hp += 10;
    }

    public void ShieldBoost(PC target)
    {
        Debug.Log(target.playerName + "의 방여력을 강화했다!!");
        target.defense += 10;
    }

    public void DamageBoost(PC target)
    {
        Debug.Log(target.playerName + "의 공격력을 강화했다!!");
        target.damage += 10;
    }
}
