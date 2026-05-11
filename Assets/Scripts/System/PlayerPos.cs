using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private GameMaster GM;
    public bool artist, coder;
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (artist)
        {
            transform.position = GM.ArtistPosition;
        }
        else
        {
            transform.position = GM.CoderPosition;
        }
    }
}
