using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchSkill : MonoBehaviourPun
{
    PhotonView PV;
    public SelectSkill selectSkill;
    private ProgrammerSkill programmerSkill;
    private void Start()
    {
        PV = photonView;
        programmerSkill = GameObject.FindGameObjectWithTag("Player2").GetComponent<ProgrammerSkill>();
    }
    public void ChangeToPosition()
    {
        if (PV.IsMine)
            programmerSkill.SwitchSkill(1);
    }
    public void ChangeToRotation()
    {
        if (PV.IsMine)
            programmerSkill.SwitchSkill(2);
    }
    public void ChangeToScale()
    {
        if (PV.IsMine)
            programmerSkill.SwitchSkill(3);
    }
}
