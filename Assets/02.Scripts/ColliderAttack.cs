using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColliderAttack : MonoBehaviour
{
    new private Collider collider;
    private Sequence attackSeqence;
    [SerializeField] private Transform trail;
    private float angle = 0f;
    private bool isAttack = false;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isAttack)
        {
            angle += Time.deltaTime * 360f;
            trail.RotateAround(transform.parent.position, Vector3.up, angle * Time.deltaTime* 2f);

            if (angle >= 360f)
            {
                angle = 0f;
                isAttack = false;
            }
        }
    }

    public void Attack()
    {
        if (attackSeqence != null)
            attackSeqence.Restart();
        else
        {
            attackSeqence = DOTween.Sequence();
            attackSeqence.SetAutoKill(false);
            attackSeqence.AppendCallback(() => collider.enabled = true);
            attackSeqence.InsertCallback(0f, () => isAttack = true);
            attackSeqence.InsertCallback(1.2f, () => collider.enabled = false);
        }
    }
}
