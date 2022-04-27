using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePos;
    private MeshRenderer muzzleFlash;

    public AudioClip fireSfx;
    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
    }

    private IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2) * 0.5f, Random.Range(0, 2) * 0.5f);
        muzzleFlash.material.mainTextureOffset = offset;
        muzzleFlash.enabled = true;

        float angle = Random.Range(0f, 360f);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        float scale = Random.Range(0.5f, 1.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        yield return new WaitForSeconds(0.2f);
        muzzleFlash.enabled = false;
    }
}
