using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobbyCustomMatch lobby;
    public string roomName;
    public string nickname;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;
    public TMP_InputField nameField;
    public TMP_InputField sizeField;
    public TMP_InputField nicknameField;
    public List<RoomInfo> roomListings;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        roomListings = new List<RoomInfo>();
        nickname = "";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Server");
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //RemoveRoomListings();
        
        int tempIndex;

        foreach(RoomInfo room in roomList)
        {
            if(roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }

            if (tempIndex != -1)
            {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
            }
            else
            {
                roomListings.Add(room);
                ListRoom(room);
            }   
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void RemoveRoomListings()
    {
        int i = 0;
        while(roomsPanel.childCount != 0)
        {
            Destroy(roomsPanel.GetChild(i).gameObject);
            i++;
        }
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.SetRoom();
        }
    }


    public void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };

        PhotonNetwork.CreateRoom(roomName, roomOps);
        Debug.Log(roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed");
        //CreateRoom();
    }

    public void OnRoomNameChanged(string inName)
    {
        roomName = nameField.text.ToString();
    }

    public void OnRoomSizeChanged(string inSize)
    {
        roomSize = int.Parse(sizeField.text.ToString());
    }

    public void OnNicknameChanged(string inSize)
    {
        nickname = nicknameField.text.ToString();
        PhotonNetwork.NickName = nickname;
    }

    public void JoinLobbyOnClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
