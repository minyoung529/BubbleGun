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

    private PaintManager paintManager;
    public PaintManager PaintManager { get => paintManager; }

    private UIManager uiManager;
    public UIManager UIManager { get => uiManager; }

    public PlayerController PlayerController { get; private set; }

    private const float MAX_X_SIZE = 10f;
    private const float MAX_Z_SIZE = 10f;

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

        if (!isSpawnMonster) return;
    }

    public void GameStart()
    {
        IsGameStart = true;

        for(int i = 0; i < 50; i++)
        {
            CreateMonster();
        }

        EventManager.TriggerEvent("GameStart");
    }

    void CreateMonster()
    {
        //float randX = Random.Range(-Mathf.Pow(MAX_X_SIZE, 2) * 0.5f, Mathf.Pow(MAX_X_SIZE, 2) * 0.5f);
        //float randZ = Random.Range(-Mathf.Pow(MAX_Z_SIZE, 2) * 0.5f, Mathf.Pow(MAX_Z_SIZE, 2) * 0.5f);

        float randX = Random.Range(-100f, 100f);
        float randZ = Random.Range(-350f, 60f);

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
}