using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class ShootingStar : ISkill
{
    public bool IsEnd { get; set; } = false;
    private GameObject spotLight;
    private float yPosition = 0f;

    private bool isUsingSkill;

    private readonly LayerMask TARGET_LAYER = LayerMask.GetMask("PLATFORM");

    private const float RADIUS = 6.5f;
    private const int BULLET_COUNT = 60;
    private const float SKILL_DURATION = 3f;

    private readonly WaitForSeconds DURATION_DELAY = new WaitForSeconds(SKILL_DURATION / BULLET_COUNT);

    public void OnEnterSkill()
    {
        GameObject prefab = Resources.Load<GameObject>("SpotLight");
        yPosition = prefab.transform.position.y;
        spotLight = PoolManager.Pop(prefab);

        Cursor.lockState = CursorLockMode.None;
    }

    public void OnExitSkill()
    {
        isUsingSkill = false;
        IsEnd = true;

        if (spotLight) PoolManager.Push(spotLight);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnStaySkill()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, TARGET_LAYER))
        {
            if (!isUsingSkill)
                MoveSpotLight(hitInfo.point);
        }

        if (Input.GetMouseButtonDown(0) && !isUsingSkill)
        {
            UseSkill();
        }

        if (Input.GetMouseButtonDown(1) && !isUsingSkill)
        {
            Debug.Log("EXIT");
            OnExitSkill();
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
        GameManager.Instance.SkillPanels[KeyCode.Q].UseSkill();
        ActEvent.ActCoroutine(ShootingCoroutine(), SKILL_DURATION);
    }

    private IEnumerator ShootingCoroutine()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Candy");
        isUsingSkill = true;
        if (spotLight == null) yield break;
        Vector3 spot = spotLight.transform.position;
        PoolManager.Push(spotLight);

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            Vector3 radomDestination = MinLib.RandomPositionInRadius(spot, RADIUS);
            radomDestination.y = 0f;
            Vector3 bulletPos = MinLib.RandomPositionInRadius(spot, RADIUS);

            PoolManager.Pop
            (
                 bulletPrefab,
                 bulletPos,
                 Quaternion.LookRotation(radomDestination - bulletPos)
            );

            yield return DURATION_DELAY;
        }

        OnExitSkill();
    }
}