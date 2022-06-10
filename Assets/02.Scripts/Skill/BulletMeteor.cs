using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMeteor : ISkill
{
    public bool IsEnd { get; set; } = false;
    private Camera mainCam;
    private GameObject spotLight;
    private float yPosition = 0f;

    private readonly KeyCode keyCode = KeyCode.Q;

    private float timer = 0f;

    public void OnEnterSkill()
    {
        mainCam = Camera.main;

        GameObject prefab = Resources.Load<GameObject>("SpotLight");
        yPosition = prefab.transform.position.y;

        spotLight = GameObject.Instantiate(prefab);
    }

    public void OnExitSkill()
    {
        IsEnd = true;
        GameObject.Destroy(spotLight);
    }

    public void OnStaySkill()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            UseSkill();
            OnExitSkill();
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKeyDown(keyCode) && timer > Time.deltaTime)
        {
            OnExitSkill();
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask("PLATFORM")))
        {
            MoveSpotLight(hitInfo.point);
        }
    }

    private void MoveSpotLight(Vector3 mousePos)
    {
        Vector3 lightPos = mousePos;
        lightPos.y = yPosition;
        spotLight.transform.position = lightPos;
    }

    private void UseSkill()
    {
        GameManager.Instance().skillPanels[keyCode].UseSkill();
    }
}
