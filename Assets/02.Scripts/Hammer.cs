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

        // 이거 시퀀스 좀 더 연구하기
        attackSeqence.AppendCallback(() => collider.enabled = false);
        attackSeqence.InsertCallback(2f, () => collider.enabled = true);
    }

    public void Attack()
    {
        attackSeqence.Restart();
    }
}
