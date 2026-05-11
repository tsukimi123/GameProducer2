using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLocalMaster : MonoBehaviour
{
    public bool ArtistIsMaster;
    public GameObject ArtistCamera1, ArtistCamera2;
    public GameObject ProgrammerCamera1, ProgrammerCamera2;
    public LocalArtistSkill artistSkill;
    public LocalInstantiateItems LocalInstantiateItems;
    public LocalProgrammerSkill LocalProgrammerSkill;
    void Update()
    {
        if (ArtistIsMaster == true)
        {
            ArtistCamera1.SetActive(true);
            ProgrammerCamera1.SetActive(false);
            artistSkill.enabled = true;
            LocalInstantiateItems.enabled = true;
            LocalProgrammerSkill.enabled = false;
        }
        else if (ArtistIsMaster == false)
        {
            ArtistCamera1.SetActive(false);
            ProgrammerCamera1.SetActive(true);
            artistSkill.enabled = false;
            LocalInstantiateItems.enabled = true;
            LocalProgrammerSkill.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ArtistIsMaster = !ArtistIsMaster;
        }
    }
}
