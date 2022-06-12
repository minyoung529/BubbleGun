using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hammer : MonoBehaviour
{
    new private Collider collider;
    private Sequence attackSeqence = DOTween.Sequence();

    private void Start()
    {
        collider = GetComponent<Collider>();

        // �̰� ������ �� �� �����ϱ�
        attackSeqence.AppendCallback(() => collider.enabled = false);
        attackSeqence.InsertCallback(2f, () => collider.enabled = true);
    }

    public void Attack()
    {
        attackSeqence.Restart();
    }
}
