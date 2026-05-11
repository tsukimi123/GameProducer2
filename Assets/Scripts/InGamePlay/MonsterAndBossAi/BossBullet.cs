using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviourPun
{
    public float damage = 20;
    public float speed = 50f;
    public float destroyTime = 20f;
    public Rigidbody rb;
    private bool Ground;
    public GameObject effect;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        StartCoroutine(destroyBullet());
        Ground = false;
    }
    private void OnCollisionEnter(Collision hitInfo)
    {
        Vector3 location = this.transform.position;
        Vector3 closestpoint = hitInfo.collider.ClosestPoint(location);
        if (Ground == false)
        {
            if (hitInfo.gameObject.CompareTag("Player"))
            {
                PlayerController player = hitInfo.transform.GetComponent<PlayerController>();
                player.TakeDamage(damage);
            }
            else if (hitInfo.gameObject.CompareTag("Player2"))
            {
                PlayerController2 player = hitInfo.transform.GetComponent<PlayerController2>();
                player.TakeDamage(damage);
            }
        }
        else if (Ground == true)
        {
            if (hitInfo.gameObject.CompareTag("Monster"))
            {
                BossAI boss = hitInfo.transform.GetComponent<BossAI>();
                boss.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        if (hitInfo.gameObject.CompareTag("Ground"))
        {
            if (!Ground)
            {
                Ground = true;
                PhotonNetwork.Instantiate(effect.name, closestpoint, Quaternion.identity);
            }
        }
    }
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
