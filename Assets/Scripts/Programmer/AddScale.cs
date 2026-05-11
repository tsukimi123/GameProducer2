using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AddScale : MonoBehaviourPun
{
    public float speed = 20f;
    public float destroyTime = 5f;
    public Rigidbody rb;
    public GameObject HitEffect;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        StartCoroutine(DestroyBullet());
    }
    public void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("DragItem"))
        {
            ChangeOwner owner = hitInfo.GetComponent<ChangeOwner>();
            if (!owner.canScale)
                return;
            PhotonNetwork.Instantiate(HitEffect.name, hitInfo.transform.position, Quaternion.identity, 0);
            if (owner != null)
            {
                hitInfo.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
                if (hitInfo.transform.localScale.x > owner.scaleX * 2f)
                {
                    hitInfo.transform.localScale = new Vector3(owner.scaleX * 2f, owner.scaleY * 2f, owner.scaleZ * 2f);
                }
            }
        }
    }
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
