using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorObject : MonoBehaviour
{
    private float posY = 0f;
    private Transform player;
    new private Animation animation;

    private const string animationName = "Generator";

    private void Awake()
    {
        EventManager<Area>.StartListening("AreaClear", OnActiveSignal);
        animation = GetComponent<Animation>();

        animation.Play();

        posY = transform.position.y;
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector3.Distance(player.position, transform.position) < 2f)
            {
                animation.Play(animationName);
            }
        }
    }

    private void OnActiveSignal(Area area)
    {
        Vector3 position = area.areaTransform.position;
        position.y = posY;
        transform.position = position;
    }
}
