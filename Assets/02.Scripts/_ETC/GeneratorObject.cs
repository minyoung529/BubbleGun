using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GeneratorObject : MonoBehaviour
{
    private float posY = 0f;
    private Transform player;
    private Animator animator;

    private readonly int actHash = Animator.StringToHash("Act");

    [SerializeField] private SpriteRenderer fButton;
    [SerializeField] private List<int> activeAreas;
    [SerializeField] private float distance = 7f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private Transform fButtonAround;

    private bool isActivate = false;


    private void Start()
    {
        EventManager<Area>.StartListening("AreaClear", OnActiveSignal);

        animator = GetComponent<Animator>();
        posY = transform.position.y;
        player = GameManager.Instance.PlayerController.transform;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isActivate) return;
        if (Vector3.Distance(player.position, transform.position) < distance)
        {
            FollowFButton();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (animator != null)
                    animator.SetTrigger(actHash);
                isActivate = true;
                GameManager.Instance.GameStart();
                fButton.DOFade(0f, 1f);
            }
        }
        else
        {
            fButton.DOFade(0f, 1f);
        }
    }

    private void OnActiveSignal(Area area)
    {
        if (!activeAreas.Contains(GameManager.Instance.AreaIndex))
        {
            gameObject.SetActive(false);
            return;
        }

        isActivate = false;
        gameObject.SetActive(true);
        Vector3 position = area.areaTransform.position;
        position.y = posY;
        transform.position = position;
    }

    private void FollowFButton()
    {
        fButton.DOFade(1f, 1f);

        Vector3 direction = (player.position - fButtonAround.position).normalized;
        Vector3 pos = direction * radius;
        pos.y = 3f;

        fButton.transform.localPosition = pos;

        fButton.transform.LookAt(player.transform.position);
        Vector3 eulerAngles = fButton.transform.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;

        fButton.transform.eulerAngles = eulerAngles;
    }
}
