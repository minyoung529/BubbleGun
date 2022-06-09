using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecracker : ISkill
{
    private GameObject firecrackerObject;
    private float timer = 0f;
    private List<MonsterCtrl> monsterCtrls;

    public void OnEnterSkill()
    {
        firecrackerObject = Resources.Load<GameObject>("Firecracker");
        Transform player = GameManager.Instance().PlayerController.transform;
        Vector3 skillPos = player.position + player.forward * 5f;
        GameObject particle = GameObject.Instantiate(firecrackerObject, skillPos, Quaternion.identity, null);
        GameObject.Destroy(particle, 3f);

        // ¿À·ù ÂÍ ÀÖÀ½...
        monsterCtrls = GameObject.FindObjectsOfType<MonsterCtrl>().ToList().FindAll(x => Vector3.Distance(x.transform.position, skillPos) < 6f);
    }

    public void OnExitSkill()
    {
        monsterCtrls.ForEach(x => x.IsMove = true);
    }

    public void OnStaySkill()
    {
        timer += Time.deltaTime;

        if (timer < 2f)
        {
            Debug.Log(monsterCtrls.Count);
            monsterCtrls.ForEach(x => x.Stun());
        }
        else
        {
            OnExitSkill();
        }
    }
}
