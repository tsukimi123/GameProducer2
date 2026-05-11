using Photon.Pun;
using UnityEngine;

public class NetworkPlayer2 : MonoBehaviourPun, IPunObservable
{
    [SerializeField] protected PlayerController2 Player2;
    [SerializeField] protected Vector3 RemotePlayerPosition;
    private void Awake()
    {
        Player2 = GetComponent<PlayerController2>();
    }
    public void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            Player2.moveSpeed = 0;
        }
        else
        {
            //Player has to go to the point
            Player2.moveSpeed = LagDistance.normalized.x;
        }
        //jump if the remote player is higher than the player on the current client
        //Player.Input.Jump = RemotePlayerPosition.y - transform.position.y > 0.2f;
        Player2.jumpForce = LagDistance.normalized.y;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
        }
    }
}