using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection2 : MonoBehaviourPun
{
    private PhotonView PV;
    public PlayerManager playerManager;
    public RoomManager roomManager;

    private GameObject Player;
    private GameObject Player2;
    //private GameObject PlayerList;
    public bool ArtistIsSelect, ProgrammerIsSelect;
    public GameObject artist, programmer;
    private GameObject[] characterList;
    public int index_2;
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject StartGameButton;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //playerManager = GameObject.FindGameObjectWithTag("aa").GetComponent<PlayerManager>();
        roomManager = GameObject.FindGameObjectWithTag("RM").GetComponent<RoomManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Player2 = GameObject.FindGameObjectWithTag("Player2");
        ArtistIsSelect = false;
        ProgrammerIsSelect = false;
    }

    private void Start()
    {
        //index_2 = PlayerPrefs.GetInt("CharacterSelected");
        index_2 = 0;

        characterList = new GameObject[transform.childCount];

        // Fill the array with models
        for(int i = 0; i<transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;
        
        //Toggle off their renderer
        /*foreach (GameObject go in characterList)
            go.SetActive(false);

        //Toggle on the selected character
        if (characterList[index_2])
            characterList[index_2].SetActive(true);*/

        if(PhotonNetwork.IsMasterClient == true)
        {
            StartGameButton.SetActive(true);
        }
    }

    public void ToggleLeft()
    {
        /*//Toggle off the current model
        characterList[index_2].SetActive(false);

        index_2--;
        if (index_2 < 0)
            index_2 = characterList.Length - 1;

        //Toggle on the new modle
        characterList[index_2].SetActive(true);*/
        index_2 = 1;
        ArtistIsSelect = true;
        ProgrammerIsSelect = false;
        artist.SetActive(true);
        programmer.SetActive(false);
    }

    public void ToggleRight()
    {
        /*//Toggle off the current model
        characterList[index_2].SetActive(false);

        index_2++;
        if (index_2 == characterList.Length)
            index_2 = 0;

        //Toggle on the new modle
        characterList[index_2].SetActive(true);*/
        index_2 = 2;
        ArtistIsSelect = false;
        ProgrammerIsSelect = true;
        artist.SetActive(false);
        programmer.SetActive(true);
    }

    public void ConfirmButton()
    {
        /*if (Player.gameObject.activeInHierarchy)
        {
            PlayerPrefs.SetInt("CharacterSelected", index_2);
            PhotonNetwork.LoadLevel(nextSceneName);
            print("Player1");
        }else if (Player2.gameObject.activeInHierarchy)
        {
            PlayerPrefs.SetInt("CharacterSelected", index_2);
            PhotonNetwork.LoadLevel(nextSceneName);
            print("Player2");
        }*/
        if (index_2 == 1)
        {
            PlayerPrefs.SetInt("CharacterSelected", index_2);
            PhotonNetwork.LoadLevel(nextSceneName);
            print("Player1");
        }
        else if (index_2 == 2)
        {
            PlayerPrefs.SetInt("CharacterSelected", index_2);
            PhotonNetwork.LoadLevel(nextSceneName);
            print("Player2");
        }
        //PlayerPrefs.SetInt("CharacterSelected", index_2);
        //PhotonNetwork.LoadLevel(2);
        //StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
