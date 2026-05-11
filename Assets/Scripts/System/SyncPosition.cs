using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : MonoBehaviourPun, IPunObservable
{
    public PhotonView PV;

    [SerializeField] private float m_Distance;

    [SerializeField] private Vector3 m_Direction;
    [SerializeField] private Vector3 m_NetworkPosition;
    [SerializeField] private Vector3 m_storedPosition;

    public bool m_SynchronizePosition;

    bool m_firstTake;

    public void Awake()
    {
        PV = GetComponent<PhotonView>();

        m_storedPosition = transform.position;
        m_NetworkPosition = Vector3.zero;
    }
    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        if (!PV.IsMine)
        {
            var disz = transform.position.z - this.m_NetworkPosition.z;
            this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);

            transform.position = Vector3.MoveTowards(transform.position, this.m_NetworkPosition, this.m_Distance * Time.deltaTime);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (this.m_SynchronizePosition)
            {
                this.m_Direction = transform.position - this.m_storedPosition;
                this.m_storedPosition = transform.position;

                stream.SendNext(transform.position);
                stream.SendNext(this.m_Direction);
            }
        }
        else
        {
            if (this.m_SynchronizePosition)
            {
                this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                this.m_Direction = (Vector3)stream.ReceiveNext();

                if (m_firstTake)
                {
                    //初始化位置
                    transform.position = this.m_NetworkPosition;
                    this.m_Distance = 0f;
                }
                else
                {
                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    this.m_NetworkPosition += this.m_Direction * lag;
                    // this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);
                }
            }
        }
    }
}
