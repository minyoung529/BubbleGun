using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 80.0f;

    private Rigidbody rigid;

    private readonly float initHp = 100.0f;
    public float currHp;

    [SerializeField] private float jumpForce;

    // Hpbar 이미지 연결
    private Image hpBar;

    [SerializeField] private UnityEvent<float, float> onMove;
    [SerializeField] private UnityEvent onJump;

    public bool IsMove { get; set; } = true;
    public static WeaponType WeaponType { get; set; } = WeaponType.Gun;

    IEnumerator Start()
    {
        float temp = rotationSpeed;
        rigid = GetComponent<Rigidbody>();

        rotationSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        rotationSpeed = temp;

        currHp = initHp;
        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        
        DisplayHP();
    }

    void Update()
    {
        if (!IsMove) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = rigid.velocity;
        moveDir.x = h;
        moveDir.z = v;

        moveDir = transform.TransformDirection(moveDir) * moveSpeed;
        moveDir.y = rigid.velocity.y;

        rigid.velocity = moveDir;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Slerp(transform.forward, FollowCamera.cameraDirection, 10f * Time.deltaTime);
        }

        Jump();

        onMove.Invoke(h, v);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PUNCH") && currHp >= 0.0f)
        {
            currHp -= 10.0f;
            Debug.Log($"Player HP = {currHp}");

            DisplayHP();

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        GameManager.Instance.IsGameOver = true;
    }

    void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHp;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpForce);
            onJump.Invoke();
        }
    }
}