using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public void HealthBoost(PC target)
    {
        Debug.Log(target.playerName + "�� ü���� ��ȭ�ߴ�!!");
        target.hp += 10;
    }

    public void ShieldBoost(PC target)
    {
        Debug.Log(target.playerName + "�� �濩���� ��ȭ�ߴ�!!");
        target.defense += 10;
    }

    public void DamageBoost(PC target)
    {
        Debug.Log(target.playerName + "�� ���ݷ��� ��ȭ�ߴ�!!");
        target.damage += 10;
    }
}
