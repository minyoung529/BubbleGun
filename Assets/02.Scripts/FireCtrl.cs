using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bulletPrefab;

    // �Ѿ� �߻� ��ǥ
    public Transform firePos;

    // �ѼҸ� ����� Ŭ��
    public AudioClip fireSfx;

    private new AudioSource audio;

    // Muzzle flash�� Mesh Renderer ������Ʈ ĳ��
    private MeshRenderer muzzleFlash;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� ���� ��, 
        if( Input.GetMouseButtonDown(0) )
        {
            Fire();
        }
    }

    void Fire()
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ����
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);

        audio.PlayOneShot(fireSfx, 1.0f);

        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        // ������ ��ǥ���� ���� �Լ��� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        // �ؽ�ó�� ������ �� ����
        muzzleFlash.material.mainTextureOffset = offset;

        // ȸ��
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);

        // ũ��
        float scale = Random.Range(0.5f, 1.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;
    }
}
