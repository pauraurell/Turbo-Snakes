using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSetting : MonoBehaviour
{
    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int multiplayerScene;
    public bool shrinking = false;
    //public Toggle toggle;

    private void Awake()
    {
        if(MultiplayerSetting.multiplayerSetting == null)
        {
            MultiplayerSetting.multiplayerSetting = this;
        }
        else
        {
            if(MultiplayerSetting.multiplayerSetting != this)
            {
                Destroy(this.gameObject);
            }
            
        }

        DontDestroyOnLoad(this.gameObject);
    }

    /*public void ChangeShrinkingValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (shrinking == false)
            {
                shrinking = true;
                toggle.isOn = true;
            }

            if (shrinking == true)
            {
                shrinking = false;
                toggle.isOn = false;
            }
        }
    }*/
}
