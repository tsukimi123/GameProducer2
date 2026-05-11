using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public GameObject Player2;
    //private GameObject PlayerList;
    public CharacterSelection2 CS;
    public static RoomManager Instance;
    public string CharacterSelect, Game, GameCopy;
    public int test_index_2;

    void Awake()
    {
        if (Instance) //check if another roommanager exists
        {
            Destroy(gameObject); //there can only be one >:)
            return;
        }
        DontDestroyOnLoad(gameObject);  //I'm the only one... :( 
        Instance = this;

        //Player = GameObject.FindGameObjectWithTag("Player");
        //Player2 = GameObject.FindGameObjectWithTag("Player2");
    }
    void Update()
    {
        test_index_2 = CS == null ? -1 : CS.index_2;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene,LoadSceneMode loadSceneMode)
    {
        if (scene.name == CharacterSelect) //we're in the select scene
        {
            CS = GameObject.FindGameObjectWithTag("CS").GetComponent<CharacterSelection2>();
            //GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            //a.tag = "aa";
            GameObject SCP = PhotonNetwork.Instantiate("SelectCharacterPlayer", Vector3.zero, Quaternion.identity);
        }

        //Player = GameObject.FindGameObjectWithTag("Player");
        //Player2 = GameObject.FindGameObjectWithTag("Player2");

        if (scene.name == Game) //we're in the game scene
        {
            //CS = GameObject.FindGameObjectWithTag("CS").GetComponent<CharacterSelection2>();

            Debug.Log(CS.index_2.ToString());

            if (CS.index_2 == 1)
            {
                GameObject PM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
                PM.tag = "PM";
            }
            else if (CS.index_2 == 2)
            {
                GameObject PM2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager2"), Vector3.zero, Quaternion.identity);
                PM2.tag = "PM2";
            }

            /*
            Player = GameObject.FindGameObjectWithTag("Player"); 
            Player2 = GameObject.FindGameObjectWithTag("Player2"); 

            //父物件是Active子物件也會是Active?
            if (Player.gameObject.activeInHierarchy) 
            {
                GameObject PM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
                PM.tag = "PM";
            }
            else if (Player2.gameObject.activeInHierarchy) 
            {
                GameObject PM2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager2"), Vector3.zero, Quaternion.identity);
                PM2.tag = "PM2";
            }
            */
        }
        if (scene.name == GameCopy)
        {
            Debug.Log(CS.index_2.ToString());
            if (CS.index_2 == 1)
            {
                GameObject PM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
                PM.tag = "PM";
            }
            else if (CS.index_2 == 2)
            {
                GameObject PM2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager2"), Vector3.zero, Quaternion.identity);
                PM2.tag = "PM2";
            }
        }
    }
}
