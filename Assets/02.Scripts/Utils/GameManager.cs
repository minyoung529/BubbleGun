using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 점수 텍스트 연결 변수
    public TMP_Text scoreText;

    // 점수 누적 변수
    private int totalScore = 0;

    // 몬스터 프리팹 연결 변수
    public GameObject monster;

    // 몬스터 생성 간격
    public float createTime = 3.0f;

    // 몬스터 출연할 위치 저장 List
    public List<Transform> points = new List<Transform>();

    // 몬스터를 미리 생성해서 저장할 List
    public List<GameObject> monsterPool = new List<GameObject>();

    // 오브젝트 풀에 생성할 몬스터 최대 갯수
    public int maxMonsters = 10;

    // 게임의 종료 여부를 저장하는 멤버 변수
    private bool isGameOver;
    public bool isSpawnMonster = true;

    private PaintManager paintManager;
    public PaintManager PaintManager { get => paintManager; }

    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if(isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<GameManager>();

            if( instance == null )
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
        DisplayScore(0);

        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);

        //Transform[] pointArray = spawnPointGroup.GetComponentsInChildren<Transform>(true);

        foreach(Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        if(!isSpawnMonster)
        {
            return;
        }
        // 일정 시간 간격으로 호출
        InvokeRepeating("CreateMonster", 2.0f, createTime);

    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for(int i=0; i<maxMonsters; ++i)
        {
            // 몬스터 생성
            var _monster = Instantiate<GameObject>(monster);

            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";

            // 몬스터 비활성화
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach(var _monster in monsterPool )
        {
            if (_monster.activeSelf == false)
                return _monster;
        }

        return null;
    }

    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totalScore:#,##0}</color>";
    }
}
