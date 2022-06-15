using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int totalScore = 0;
    public List<GameObject> monsterPrefabs;

    public List<MonsterCtrl> CurrentMonster { get; private set; } = new List<MonsterCtrl>();
    public Dictionary<KeyCode, SkillPanel> SkillPanels { get; private set; } = new Dictionary<KeyCode, SkillPanel>();

    public bool isSpawnMonster = true;

    public Transform[] areaTransforms;
    private Vector3 areaLeftTop;
    private Vector3 areaRightBottom;
    private int areaIndex = -1;

    public PaintManager PaintManager { get; private set; }
    public UIManager UIManager { get; private set; }

    public PlayerController PlayerController { get; private set; }

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

    private int maxEnemyCount = 0;
    private int deadEnemyCount = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;

        PaintManager = FindObjectOfType<PaintManager>();
        UIManager = FindObjectOfType<UIManager>();
        PlayerController = FindObjectOfType<PlayerController>();

        MainCam = Camera.main;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        AddScore(0);
    }

    public void GameStart()
    {
        IsGameStart = true;
        EventManager.TriggerEvent("GameStart");

        ClearArea();
    }

    public void ClearArea()
    {
        Transform curArea = areaTransforms[++areaIndex];

        areaLeftTop = curArea.position;
        areaRightBottom = curArea.position;
        
        areaLeftTop.x -= curArea.localScale.x * 5f;
        areaLeftTop.z += curArea.localScale.z * 5f;

        areaRightBottom.x += curArea.localScale.x * 5f;
        areaRightBottom.z -= curArea.localScale.z * 5f;

        for (int i = 0; i < 50; i++)
        {
            maxEnemyCount = 50;
            if (isSpawnMonster)
                CreateMonster(areaLeftTop, areaRightBottom);
        }

        UIManager.UpdateInfo(maxEnemyCount, deadEnemyCount);
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

    public void OnEnemyDie()
    {
        deadEnemyCount++;
        UIManager.UpdateInfo(maxEnemyCount, deadEnemyCount);
    }
}