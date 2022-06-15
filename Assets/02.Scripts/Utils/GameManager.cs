using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int totalScore = 0;

    public List<GameObject> monsterPrefabs;

    // 몬스터 생성 간격
    [SerializeField] private float createTime = 1.50f;

    public List<MonsterCtrl> CurrentMonster { get; private set; } = new List<MonsterCtrl>();
    public Dictionary<KeyCode, SkillPanel> SkillPanels { get; private set; } = new Dictionary<KeyCode, SkillPanel>();

    // 게임의 종료 여부를 저장하는 멤버 변수
    private bool isGameOver;
    public bool isSpawnMonster = true;

    public Transform[] areaTransforms;
    private int transformIndex = -1;
    private Vector3 areaLeftTop;
    private Vector3 areaRightBottom;

    private PaintManager paintManager;
    public PaintManager PaintManager { get => paintManager; }

    private UIManager uiManager;
    public UIManager UIManager { get => uiManager; }

    public PlayerController PlayerController { get; private set; }

    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }
    public bool IsGameStart { get; set; } = false;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("GameMgr");
                    instance = container.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public Camera MainCam { get; set; }

    void Awake()
    {
        if (instance == null)
            instance = this;

        paintManager = FindObjectOfType<PaintManager>();
        uiManager = FindObjectOfType<UIManager>();
        PlayerController = FindObjectOfType<PlayerController>();

        MainCam = Camera.main;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        // 스코어 점수 출력
        AddScore(0);

    }

    public void GameStart()
    {
        IsGameStart = true;
        EventManager.TriggerEvent("GameStart");

        if (!isSpawnMonster) return;
        ClearArea();
    }

    public void ClearArea()
    {
        transformIndex++;
        Transform curArea = areaTransforms[transformIndex];

        areaLeftTop = curArea.position;
        areaLeftTop.x -= curArea.localScale.x * 5f;
        areaLeftTop.z += curArea.localScale.z * 5f;

        areaRightBottom = curArea.position;
        areaRightBottom.x += curArea.localScale.x * 5f;
        areaRightBottom.z -= curArea.localScale.z * 5f;

        Debug.DrawRay(new Vector3(areaRightBottom.x, 0f, areaRightBottom.z), Vector3.up * 999f, Color.yellow, 999f);
        Debug.DrawRay(new Vector3(areaLeftTop.x, 0f, areaLeftTop.z), Vector3.up * 999f, Color.yellow, 999f);

        for (int i = 0; i < 50; i++)
        {
            CreateMonster(areaLeftTop, areaRightBottom);
        }
    }

    private void CreateMonster(Vector3 lt, Vector3 rb)
    {
        float randX = Random.Range(lt.x, rb.x);
        float randZ = Random.Range(lt.z, rb.z);

        GameObject _monster = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
        _monster = Instantiate(_monster, new Vector3(randX, 0f, randZ), Quaternion.identity);

        MonsterCtrl monster = _monster.GetComponent<MonsterCtrl>();

        if (!CurrentMonster.Contains(monster))
            CurrentMonster.Add(monster);
    }

    public void AddScore(int score)
    {
        totalScore += score;
        UIManager.UpdateScore(totalScore);
    }

    public Vector3 ClampArea(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, areaLeftTop.x, areaRightBottom.x);
        position.z = Mathf.Clamp(position.z, areaRightBottom.z, areaLeftTop.z);

        return position;
    }
}