using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public float secondsCount;
    private int minuteCount;
    private int hourCount;
    public GameObject boost;
    public int firstTime;
    public int secondTime;
    public bool randomDone = true;
    public bool randomAdditionDone = true;
    private int randomTime;
    public bool started = false;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI currentText;
    private PlayerList lists;

    private int currentRound = 1;
    public int numberOfRounds = 3;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        randomTime = Random.Range(firstTime, secondTime);
        timeText = GameObject.Find("timeText").GetComponent<TextMeshProUGUI>();
        //currentText = GameObject.Find("currentText").GetComponent<TextMeshProUGUI>();
       // lists = GameObject.Find("PlayerList").GetComponent<PlayerList>();
    }

    void Update()
    {
        //currentText.text = currentRound.ToString();
        UpdateTimer();
        if (secondsCount <= 4)
        {
            int tempTime = (int)secondsCount;
            timeText.text = (-(tempTime-3)).ToString();
        }
        if (secondsCount >= 4 && secondsCount < 5) 
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;
                if (g.tag == "User")
                {
                    g.transform.GetChild(0).GetComponent<PlayerMovement>().input = true;
                }
            }
            timeText.transform.parent.gameObject.SetActive(false);
            started = true;
        }

        if ((int)secondsCount == randomTime)
        {
            SpawnRandomBoost();
        }

        RandmomizeBoostTiming();

        if (GameObject.Find("PlayerList").GetComponent<PlayerList>().deadPlayersList.Count == GameObject.Find("PlayerList").GetComponent<PlayerList>().playerList.Count - 1) 
        {
            //ResetPlayers();
            //DestroyBoosts();
            //secondsCount = 0;
        }

    }

    public void ResetPlayers()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.tag == "User")
            {
                g.transform.GetChild(1).GetComponent<LineRenderer>().positionCount = 0;

                g.transform.position = g.transform.GetChild(0).GetComponent<PlayerMovement>().initialPos;
                g.transform.rotation = g.transform.GetChild(0).GetComponent<PlayerMovement>().initialRot;
                g.transform.GetChild(0).GetComponent<PlayerMovement>().input = true;
            }
        }


    }

    public void DestroyBoosts()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.tag == "User")
            {
                PhotonNetwork.Destroy(g);
            }
        }
    }

    public void RandmomizeBoostTiming() 
    {
        if (randomDone == false)
        {
            randomTime = Random.Range(firstTime, secondTime);
            randomDone = true;
        }
    }

    public string GetRandomBoost()
    {
        int randomBoost = Random.Range(0, 3);
        if (randomBoost == 0)
        {
            return "Turbo";
        }
        else if (randomBoost == 1)
        {
            return "Brake";
        }
        else if (randomBoost == 2)
        {
            return "Points";
        }
        return "Turbo";
    }

    public void SpawnRandomBoost()
    {
        Vector3 randomBoostSpawn = new Vector3(Random.Range(-7.3f, 4.3f), Random.Range(4.4f, -4.3f), 0);

        string Boost = GetRandomBoost();

        boost = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Boost), randomBoostSpawn, GameSetup.GS.spawnPoints[0].rotation, 0);
        AddRandomTime();
    }

    public void AddRandomTime()
    {
        randomDone = false;
        randomAdditionDone = false;
        if (randomAdditionDone == false)
        {
            int randomTimeAddition = Random.Range(9, 18);
            firstTime += randomTimeAddition;
            secondTime += randomTimeAddition;
            randomAdditionDone = true;
        }
    }

    public void UpdateTimer()
    {
        secondsCount += Time.deltaTime;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount %= 60;
            if (minuteCount >= 60)
            {
                hourCount++;
                minuteCount %= 60;
            }
        }
    }

}
