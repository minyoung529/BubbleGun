using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 점수 텍스트 연결 변수
    public Text scoreText;

    // 점수 누적 변수
    private int totalScore = 0;

    // 몬스터 프리팹 연결 변수
    public List<GameObject> monster;

    // 몬스터 생성 간격
    public float createTime = 3.0f;

    // 몬스터를 미리 생성해서 저장할 List
    public List<GameObject> monsterPool = new List<GameObject>();

    public Dictionary<KeyCode, SkillPanel> skillPanels = new Dictionary<KeyCode, SkillPanel>();

    // 오브젝트 풀에 생성할 몬스터 최대 갯수
    public int maxMonsters = 10;

    // 게임의 종료 여부를 저장하는 멤버 변수
    private bool isGameOver;
    public bool isSpawnMonster = true;

    private PaintManager paintManager;
    public PaintManager PaintManager { get => paintManager; }

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

    private static GameManager instance;
    public static GameManager Instance()
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

    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(this.gameObject);

        paintManager = FindObjectOfType<PaintManager>();
    }

    void Start()
    {
        // 스코어 점수 출력
        AddScore(0);

        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        if (!isSpawnMonster) return;

        // 일정 시간 간격으로 호출
        InvokeRepeating("CreateMonster", 2.0f, createTime);

        PlayerController = FindObjectOfType<PlayerController>();
    }

    void CreateMonster()
    {
        float randX = Random.Range(-Mathf.Pow(MAX_X_SIZE, 2) * 0.5f, Mathf.Pow(MAX_X_SIZE, 2) * 0.5f);
        float randZ = Random.Range(-Mathf.Pow(MAX_Z_SIZE, 2) * 0.5f, Mathf.Pow(MAX_Z_SIZE, 2) * 0.5f);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(new Vector3(randX, 0f, randZ), Quaternion.identity);

        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; ++i)
        {
            // 몬스터 생성
            var _monster = Instantiate(monster[Random.Range(0, monster.Count)]);

            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";

            // 몬스터 비활성화
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if (_monster.activeSelf == false)
                return _monster;
        }

        return null;
    }

    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = $"x {score}";
    }
}
