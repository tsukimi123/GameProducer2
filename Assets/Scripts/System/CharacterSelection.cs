using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviourPun
{
    PhotonView PV;
    public PlayerManager playerManager;
    public PlayerManager2 playerManager2;

    private GameObject[] characterList;
    private int index;

    [SerializeField] GameObject StartGameButton;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = GameObject.FindGameObjectWithTag("aa").GetComponent<PlayerManager>();
    }

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");

        characterList = new GameObject[transform.childCount];

        // Fill the array with models
        for(int i = 0; i<transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;
        
        //Toggle off their renderer
        foreach (GameObject go in characterList)
            go.SetActive(false);

        //Toggle on the selected character
        if (characterList[index])
            characterList[index].SetActive(true);
    }

    public void ToggleLeft()
    {
        //Toggle off the current model
        characterList[index].SetActive(false);

        index--;
        if (index < 0)
            index = characterList.Length - 1;

        //Toggle on the new modle
        characterList[index].SetActive(true);
    }

    public void ToggleRight()
    {
        //Toggle off the current model
        characterList[index].SetActive(false);

        index++;
        if (index == characterList.Length)
            index = 0;

        //Toggle on the new modle
        characterList[index].SetActive(true);
    }

    public void ConfirmButton()
    {
        if (characterList[0])
        {
            playerManager.CreatController();
            PhotonNetwork.LoadLevel(2);
        }

        if (characterList[1])
        {
            playerManager2.CreatController2();
            PhotonNetwork.LoadLevel(2);
        }

        PlayerPrefs.SetInt("CharacterSelected", index);
        PhotonNetwork.LoadLevel(2);
        //StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
