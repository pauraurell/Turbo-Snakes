using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sizeText;
    public string roomName;
    public int roomSize;

    public void SetRoom()
    {
        nameText.text = roomName;
        sizeText.text = roomSize.ToString();
    }

    public void JoinRoomOnClicked()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

}
