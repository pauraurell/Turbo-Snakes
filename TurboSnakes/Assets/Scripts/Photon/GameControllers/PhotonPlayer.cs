using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject Player;
    public GameObject MatchManager;
    public GameObject List;
    public int Points;
    

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            Player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), GameSetup.GS.spawnPoints[PV.Owner.ActorNumber - 1].position, GameSetup.GS.spawnPoints[PV.Owner.ActorNumber - 1].rotation, 0);
            
            //PV.RPC("AddGameObjToList", RpcTarget.All);

            Vector3 pos = new Vector3(0, 0, 0);
            Instantiate(MatchManager, pos, Quaternion.identity);

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void AddGameObjToList() 
    {
        GameObject.Find("PlayerList").GetComponent<PlayerList>().playerList.Add(Player);
    }

}
