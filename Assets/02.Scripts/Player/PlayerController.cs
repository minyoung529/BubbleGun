using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10;
    private float rotationSpeed = 130;

    private Rigidbody rigid;

    public float jumpForce = 200f;

    private readonly float initHp = 100.0f;
    private float curHp;

    public LayerMask platform;

    public static WeaponType WeaponType { get; private set; } = WeaponType.Gun;

    [SerializeField] private UnityEvent<float, float> onMove;
    [SerializeField] private UnityEvent onJump;
    [SerializeField] private UnityEvent<WeaponType> onChangeWeapon;

    [SerializeField] private AudioClip hurtClip;

    IEnumerator Start()
    {
        float temp = rotationSpeed;
        rigid = GetComponent<Rigidbody>();

        rotationSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        rotationSpeed = temp;

        curHp = initHp;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.GameState != GameState.Game &&
            GameManager.Instance.GameState != GameState.Ready) return;

        Move();
        Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PUNCH") && curHp >= 0.0f)
        {
            GameManager.Instance.MainCam.transform.DOShakePosition(0.2f, 0.3f);

            curHp -= 10.0f;
            GameManager.Instance.UIManager.UpdateHp(curHp, initHp);
            SoundManager.Instance.PlayOneShot(SoundType.Voice, hurtClip);

            if (curHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    #region Abount Movement
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = Vector3.zero;
        moveDir.x = h;
        moveDir.z = v;

        moveDir = transform.TransformDirection(moveDir) * moveSpeed;
        moveDir.y = rigid.velocity.y;

        rigid.velocity = moveDir;

        if (moveDir.sqrMagnitude > 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward, FollowCamera.cameraDirection, 10f * Time.deltaTime);
        }

        if (GameManager.Instance.GameState == GameState.Game)
            transform.position = GameManager.Instance.ClampArea(transform.position);

        onMove.Invoke(h, v);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.OverlapBox(transform.position, Vector3.one * 0.5f, Quaternion.identity, platform).Length == 0) return;

            rigid.AddForce(Vector3.up * jumpForce);
            onJump.Invoke();
        }
    }
    #endregion

    public void SetWeaponType(WeaponType weaponType)
    {
        WeaponType = weaponType;
        onChangeWeapon.Invoke(weaponType);
    }

    void PlayerDie()
    {
        EventManager.TriggerEvent("GameOver");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }
}