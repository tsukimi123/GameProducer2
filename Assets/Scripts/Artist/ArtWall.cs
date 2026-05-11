using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ArtWall : MonoBehaviourPun, IDamageable
{
    public float wallHp = 12f;
    private PhotonView PV;
    private Material myMaterial;
    private float _disapper;
    public bool needDestory = false;
    private float timer;
    public float DestoryTime;
    public string myName;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        myMaterial = this.GetComponent<MeshRenderer>().material;
        _disapper = 0f;
    }
    private void Update()
    {
        myMaterial.SetFloat("_disapper", _disapper);
        if (needDestory)
        {
            timer += Time.deltaTime;
            if (timer > DestoryTime)
            {
                StartCoroutine(Disapper());
            }
        }
    }
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        wallHp -= damage;
        if (wallHp <= 0)
        {
            StartCoroutine(Disapper());
        }
    }
    IEnumerator Disapper()
    {
        DOTween.To(() => _disapper, x => _disapper = x, 1.0f, 1.1f);
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(this.gameObject);
    }
}