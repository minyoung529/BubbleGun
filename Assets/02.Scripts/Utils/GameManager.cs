using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int totalScore = 0;
    public GameObject[] monsterPrefabs;

    public List<MonsterCtrl> CurrentMonster { get; private set; } = new List<MonsterCtrl>();
    public Dictionary<KeyCode, SkillPanel> SkillPanels { get; private set; } = new Dictionary<KeyCode, SkillPanel>();

    public bool isSpawnMonster = true;

    public List<Area> areas;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            deadEnemyCount = maxEnemyCount - 1;
            OnEnemyDie();
        }
    }

    public void GameStart()
    {
        IsGameStart = true;
        EventManager.TriggerEvent("GameStart");

        ClearArea();
    }

    public void ClearArea()
    {
        Transform curArea = areas[++areaIndex].areaTransform;

        areaLeftTop = curArea.position;
        areaRightBottom = curArea.position;
        
        areaLeftTop.x -= curArea.localScale.x * 5f;
        areaLeftTop.z += curArea.localScale.z * 5f;

        areaRightBottom.x += curArea.localScale.x * 5f;
        areaRightBottom.z -= curArea.localScale.z * 5f;

        for (int i = 0; i < monsterPrefabs.Length; ++i)
        {
            for(int j = 0; j < areas[areaIndex].monsterCount[i]; ++j)
            {
                if (isSpawnMonster)
                    CreateMonster(monsterPrefabs[i], areaLeftTop, areaRightBottom);

                ++maxEnemyCount;
            }
        }

        UIManager.ShowInfoText("영역 내 모든 몬스터를 공격해 '껌'으로 만드세요. ()");
        UIManager.UpdateInfo(maxEnemyCount, deadEnemyCount);
    }

    private void CreateMonster(GameObject _monster, Vector3 lt, Vector3 rb)
    {
        float randX = Random.Range(lt.x, rb.x);
        float randZ = Random.Range(lt.z, rb.z);

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

        if(deadEnemyCount == maxEnemyCount)
        {
            EventManager<Area>.TriggerEvent("AreaClear", areas[++areaIndex]);
            UIManager.ShowInfoText("신호기의 빛을 따라가세요.");
        }

        UIManager.UpdateInfo(maxEnemyCount, deadEnemyCount);
    }
}