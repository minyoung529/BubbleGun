using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMeteor : ISkill
{
    public bool IsEnd { get; set; } = false;
    private Camera mainCam;
    private GameObject spotLight;

    public void OnEnterSkill()
    {
        mainCam = Camera.main;
        spotLight = GameObject.Instantiate(Resources.Load<GameObject>("SpotLight"));
    }

    public void OnExitSkill()
    {
        IsEnd = true;
    }

    public void OnStaySkill()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("END");
            OnExitSkill();
        }

        RaycastHit hitInfo;
        if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask("PLATFORM")))
        {
            MoveSpotLight(mainCam.ScreenToWorldPoint(hitInfo.point));
        }
    }

    private void MoveSpotLight(Vector3 mousePos)
    {
        Vector3 lightPos = mousePos;
        lightPos.y = spotLight.transform.position.y;
        spotLight.transform.position = lightPos;
    }
}
