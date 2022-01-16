using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    public static PlayerMovement PM;
    public MatchManager MM;
    public int points = 0;
    private CharacterController myCC;
    public float initialSpeed = 1.2f;
    public float currentSpeed;
    public float rotationSpeed = 26f;
    public float horizontal;
    public bool dead = false;
    public bool input = false;
    public GameObject tail;

    public Slider turboSlider;
    public float maxTurbo = 100.0f;
    public float currentTurbo;
    public float turboSpeed;

    public Slider breakSlider;
    public float maxBreak = 100.0f;
    public float currentBreak;
    public float breakSpeed;
    public Vector3 initialPos;
    public Quaternion initialRot;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("ChangeGOName", RpcTarget.All, PV.Owner.NickName);
        }


        if (PV.IsMine)
        {
            PV.RPC("SetInitialPos", RpcTarget.All, transform.position, transform.rotation);
        }

        if (PV.IsMine)
        {
            PV.RPC("SetInitialSpeeds", RpcTarget.All, initialSpeed, maxTurbo, maxBreak);
        }

    }

    // Update is called once per frame
    void Update()
    {
     
        if (PV.IsMine && input == true)
        {
            Movement();
            PV.RPC("UpdatePositions", RpcTarget.All, transform.position, transform.rotation);
            transform.Translate(transform.up * Time.deltaTime * currentSpeed, Space.Self);
            transform.Rotate(Vector3.forward * -horizontal * rotationSpeed * 4 * Time.deltaTime);
            horizontal = 0;
            //PV.RPC("UpdateSpeed", RpcTarget.All, movementSpeed);
        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLIDER");
        if (col.tag == "killsPlayer")
        {

            if (PV.IsMine)
            {
                currentSpeed = 0f;
                rotationSpeed = 0f;
                dead = true;
                PV.RPC("UpdateDeath", RpcTarget.All, dead);
                
            }
            //GameObject.FindObjectOfType<GameManager>().EndGame();
        }

        if (col.tag == "Points")
        {
            if (PV.IsMine)
            {
                points += 10;
                PV.RPC("UpdatePoints", RpcTarget.All, points);
                
                GameObject.Find("PlayerList").GetComponent<PlayerList>().ClearPlayerListings();
                GameObject.Find("PlayerList").GetComponent<PlayerList>().ListPlayers();
            }

            Destroy(col.gameObject);
        }

        if (col.tag == "Turbo")
        {
            if (PV.IsMine)
            {
                currentTurbo += 10;
                GameObject.Find("Canvas").transform.GetChild(3).GetComponent<TurboBar>().turboSlider.value = currentTurbo / maxTurbo;
                PV.RPC("UpdateTurbo", RpcTarget.All, currentTurbo);
                
            }
            Destroy(col.gameObject);
        }

        if (col.tag == "Break")
        {
            if (PV.IsMine)
            {
                currentBreak += 10;
                GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BreakBar>().breakSlider.value = currentBreak / maxBreak;
                PV.RPC("UpdateBreak", RpcTarget.All, currentBreak);
                
            }
            Destroy(col.gameObject);
        }
    }

    void Movement()
    {
        if (!dead)
        {
            if (Input.GetKey(KeyCode.A))
            {
                horizontal = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontal = 1;
                horizontal = 1;
            }
            if (Input.GetKey(KeyCode.W) && currentTurbo > 0)
            {
                if (PV.IsMine)
                {
                    currentSpeed = turboSpeed;
                    currentTurbo -= 1;
                    GameObject.Find("Canvas").transform.GetChild(3).GetComponent<TurboBar>().turboSlider.value = currentTurbo;
                    PV.RPC("UpdateTurbo", RpcTarget.All, currentTurbo);
                    PV.RPC("UpdateSpeed", RpcTarget.All, currentSpeed);
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                if (PV.IsMine)
                {
                    currentSpeed = initialSpeed;               
                    PV.RPC("UpdateSpeed", RpcTarget.All, currentSpeed);
                }
            }
            if (Input.GetKey(KeyCode.S) && currentBreak > 0)
            {
                if (PV.IsMine)
                {
                    currentSpeed = breakSpeed;
                    currentBreak -= 1;
                    GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BreakBar>().breakSlider.value = currentBreak;
                    PV.RPC("UpdateBreak", RpcTarget.All, currentBreak);
                    PV.RPC("UpdateSpeed", RpcTarget.All, currentSpeed);
                }
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                if (PV.IsMine)
                {
                    currentSpeed = initialSpeed;
                    PV.RPC("UpdateSpeed", RpcTarget.All, currentSpeed);
                }
            }
        }
    }

    [PunRPC]
    public void UpdatePoints(int newPoints) 
    {
        points = newPoints;
    }

    [PunRPC]
    public void UpdatePositions(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    [PunRPC]
    public void SetInitialPos(Vector3 pos, Quaternion rot)
    {
        initialPos = pos;
        initialRot = rot;
    }

    [PunRPC]
    public void SetInitialSpeeds(float initialSpeed, float maxTurbo, float maxBreak)
    {
        currentSpeed = initialSpeed;
        currentTurbo = maxTurbo;
        currentBreak = maxBreak;
    }

    [PunRPC]
    public void ChangeGOName(string newName)
    {
        this.transform.parent.name = newName;
    }

    [PunRPC]
    public void UpdateDeath(bool newDead)
    {
        dead = newDead;
        GameObject.Find("PlayerList").GetComponent<PlayerList>().deadPlayersList.Add(this.transform.parent.gameObject);
    }

    [PunRPC]
    public void UpdateTurbo(float newTurbo)
    {
        currentTurbo = newTurbo;
    }

    [PunRPC]
    public void UpdateBreak(float newBreak)
    {
        currentBreak = newBreak;
    }

    [PunRPC]
    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }
}



