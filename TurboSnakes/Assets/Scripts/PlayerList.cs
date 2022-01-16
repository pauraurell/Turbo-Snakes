using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    private PhotonView PV;
    public GameObject playerListingPrefab;
    public GameObject Player;
    private string name;
    private bool listed =false;
    public List<GameObject> playerList;
    public List<GameObject> deadPlayersList;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 40 == 0)
        {
            ClearPlayerListings();
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;
                if(g.tag == "User")
                {
                    playerList.Add(g);
                }
            }
            
            ListPlayers();
            //listed = true;
        }
        
    }

    public void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (GameObject player in playerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, GameObject.Find("Canvas").transform.GetChild(0));
                TextMeshProUGUI tempText = tempListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                tempText.text = player.name + "     " + player.transform.GetChild(0).GetComponent<PlayerMovement>().points.ToString();
                /*if (PV.IsMine)
                {
                    PV.RPC("ChangeName", RpcTarget.All, text);
                }*/
            }
        }
    }

    public void ClearPlayerListings()
    {
        for (int i = GameObject.Find("Canvas").transform.GetChild(0).childCount - 1; i >= 0; i--)
        {
            Destroy(GameObject.Find("Canvas").transform.GetChild(0).GetChild(i).gameObject);
        }
        playerList.Clear();
    }

    [PunRPC]
    public void ChangeName(string newText) 
    {
        GameObject tempListing = Instantiate(playerListingPrefab, GameObject.Find("Canvas").transform.GetChild(0));
        TextMeshProUGUI tempText = tempListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tempText.text = newText;
    }
}
