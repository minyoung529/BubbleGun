using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject monster;

    public float createTime = 3f;

    public List<Transform> points = new List<Transform>();

    //固府 积己, 历厘且 府胶飘
    public List<GameObject> monsterPool = new List<GameObject>();
    public int maxMonsters = 10;

    private bool isGameOver = false;
    public bool IsGameOver
    {
        get => isGameOver;
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
    private static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        //spawnPointGroup.GetComponentsInChildren(points);
        CreateMonsterPool();
        InvokeRepeating("CreateMonster", 3f, 3f);
    }

    private void CreateMonster()
    {
        int index = Random.Range(0, points.Count);

        GameObject obj = GetMonsterInPool();

        if (obj == null)
        {
            obj = Instantiate(monster, points[index].position, points[index].rotation);
            monsterPool.Add(obj);
        }
        else
        {
            obj.transform.SetPositionAndRotation(points[index].position, points[index].rotation);
            obj.SetActive(true);
        }
    }

    private void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            var _monster = Instantiate(monster);
            _monster.name = $"Monster_{i:00}";
            _monster.gameObject.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var obj in monsterPool)
        {
            if (!obj.activeSelf)
                return obj;
        }

        return null;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
