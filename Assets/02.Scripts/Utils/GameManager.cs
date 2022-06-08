using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ���� �ؽ�Ʈ ���� ����
    public Text scoreText;

    // ���� ���� ����
    private int totalScore = 0;

    // ���� ������ ���� ����
    public List<GameObject> monster;

    // ���� ���� ����
    public float createTime = 3.0f;

    // ���͸� �̸� �����ؼ� ������ List
    public List<GameObject> monsterPool = new List<GameObject>();

    public Dictionary<KeyCode, SkillPanel> skillPanels = new Dictionary<KeyCode, SkillPanel>();

    // ������Ʈ Ǯ�� ������ ���� �ִ� ����
    public int maxMonsters = 10;

    // ������ ���� ���θ� �����ϴ� ��� ����
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
        // ���ھ� ���� ���
        AddScore(0);

        // ���� ������Ʈ Ǯ ����
        CreateMonsterPool();

        if (!isSpawnMonster) return;

        // ���� �ð� �������� ȣ��
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
            // ���� ����
            var _monster = Instantiate(monster[Random.Range(0, monster.Count)]);

            // ���� �̸� ����
            _monster.name = $"Monster_{i:00}";

            // ���� ��Ȱ��ȭ
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
