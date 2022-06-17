using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int totalScore = 0;
    public GameObject[] monsterPrefabs;

    public List<MonsterCtrl> CurrentMonster { get; private set; } = new List<MonsterCtrl>();
    public Dictionary<KeyCode, SkillPanel> SkillPanels { get; private set; } = new Dictionary<KeyCode, SkillPanel>();

    public bool isSpawnMonster = true;

    [SerializeField] private List<Area> areas;
    private Vector3 areaLeftTop;
    private Vector3 areaRightBottom;
    public int AreaIndex { get; private set; } = 0;
    public Area CurrentArea
    {
        get => areas[AreaIndex];
    }
    public bool IsClear
    {
        get => AreaIndex >= areas.Count - 1;
    }

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
    public PaintManager PaintManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public PlayerController PlayerController { get; private set; }


    public GameState GameState { get; set; }
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

        EventManager.StartListening("GameOver", OnGameOver);
    }

    void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isSpawnMonster)
            {
                foreach (var m in CurrentMonster)
                {
                    if (m.MonsterState != MonsterCtrl.State.DIE)
                    {
                        m.MonsterState = MonsterCtrl.State.DIE;
                    }
                }
            }
            else
            {
                deadEnemyCount = maxEnemyCount - 1;
                OnEnemyDie();
            }
        }
    }

    public void GameStart()
    {
        GameState = GameState.Game;
        EventManager.TriggerEvent("GameStart");

        ClearArea();
    }

    public void ClearArea()
    {
        Transform curArea = areas[AreaIndex].areaTransform;

        areaLeftTop = curArea.position;
        areaRightBottom = curArea.position;

        areaLeftTop.x -= curArea.localScale.x * 5f;
        areaLeftTop.z += curArea.localScale.z * 5f;

        areaRightBottom.x += curArea.localScale.x * 5f;
        areaRightBottom.z -= curArea.localScale.z * 5f;

        if(AreaIndex + 1 == areas.Count)
        {
            EventManager.TriggerEvent("Win");
            Debug.Log("clear");
        }

        StartCoroutine(GenerateMonsterCoroutine());

        foreach (MonsterGenerate info in CurrentArea.monsterGenerates)
            maxEnemyCount += info.count;

        UIManager.ShowInfoText(string.Format(areas[AreaIndex].infoMessage, maxEnemyCount));
    }

    private void CreateMonster(GameObject _monster, Vector3 lt, Vector3 rb)
    {
        float randX = Random.Range(lt.x, rb.x);
        float randZ = Random.Range(lt.z, rb.z);

        _monster = PoolManager.Instantiate(_monster, new Vector3(randX, 0f, randZ), Quaternion.identity);

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

        if (deadEnemyCount == maxEnemyCount)
        {
            GameState = GameState.Ready;

            if (areas.Count > AreaIndex)
            {
                EventManager<Area>.TriggerEvent("AreaClear", areas[++AreaIndex]);
            }

            UIManager.ShowInfoText("신호기의 빛을 따라가세요.&");
        }

        UIManager.UpdateInfo(maxEnemyCount, deadEnemyCount);
    }

    private IEnumerator GenerateMonsterCoroutine()
    {
        int index = 0;
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(2f);

        while (index < CurrentArea.monsterGenerates.Length)
        {
            yield return new WaitForSeconds(CurrentArea.monsterGenerates[index].time);

            MonsterGenerate generateInfo = CurrentArea.monsterGenerates[index];

            if (generateInfo.count == 0)
            {
                yield break;
            }

            for (int i = 0; i < generateInfo.count; ++i)
            {
                if (isSpawnMonster)
                    CreateMonster(monsterPrefabs[(int)generateInfo.monsterType], areaLeftTop, areaRightBottom);
             
                yield return delay;
            }
            index++;
        }
    }

    private void OnGameOver()
    {
        GameState = GameState.None;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameOver", OnGameOver);
    }
}