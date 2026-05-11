using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MaoMaobullet : MonoBehaviourPun
{
    public float damage = 20;
    public float speed = 20f;
    public float destroyTime = 5f;
    public Rigidbody rb;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        StartCoroutine(DestroyBullet());
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            PlayerController player = hitInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        else if (hitInfo.CompareTag("Player2"))
        {
            PlayerController2 player2 = hitInfo.GetComponent<PlayerController2>();
            if (player2 != null)
            {
                player2.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
