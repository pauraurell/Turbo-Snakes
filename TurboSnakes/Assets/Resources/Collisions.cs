using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    private void Update()
    {
        //transform.rotation.Set(0,0,transform.parent.transform.rotation.z*2,0);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLIDER");
        if (col.tag == "killsPlayer")
        {

            if (transform.parent.GetComponent<PlayerMovement>().PV.IsMine)
            {
                transform.parent.GetComponent<PlayerMovement>().currentSpeed = 0f;
                transform.parent.GetComponent<PlayerMovement>().rotationSpeed = 0f;
                transform.parent.GetComponent<PlayerMovement>().dead = true;
                transform.parent.GetComponent<PlayerMovement>().PV.RPC("UpdateDeath", RpcTarget.All, transform.parent.GetComponent<PlayerMovement>().dead);

            }
            //GameObject.FindObjectOfType<GameManager>().EndGame();
        }

        if (col.tag == "Points")
        {
            if (transform.parent.GetComponent<PlayerMovement>().PV.IsMine)
            {
                transform.parent.GetComponent<PlayerMovement>().points += 10;
                transform.parent.GetComponent<PlayerMovement>().PV.RPC("UpdatePoints", RpcTarget.All, transform.parent.GetComponent<PlayerMovement>().points);

                GameObject.Find("PlayerList").GetComponent<PlayerList>().ClearPlayerListings();
                GameObject.Find("PlayerList").GetComponent<PlayerList>().ListPlayers();
                transform.parent.GetComponent<PlayerMovement>().source1.Play();
            }

            Destroy(col.gameObject);
        }

        if (col.tag == "Turbo")
        {
            if (transform.parent.GetComponent<PlayerMovement>().PV.IsMine)
            {
                transform.parent.GetComponent<PlayerMovement>().currentTurbo += 10;
                GameObject.Find("Canvas").transform.GetChild(3).GetComponent<TurboBar>().turboSlider.value = transform.parent.GetComponent<PlayerMovement>().currentTurbo / transform.parent.GetComponent<PlayerMovement>().maxTurbo;
                transform.parent.GetComponent<PlayerMovement>().PV.RPC("UpdateTurbo", RpcTarget.All, transform.parent.GetComponent<PlayerMovement>().currentTurbo);
                transform.parent.GetComponent<PlayerMovement>().source2.Play();
            }
            Destroy(col.gameObject);
        }

        if (col.tag == "Brake")
        {
            if (transform.parent.GetComponent<PlayerMovement>().PV.IsMine)
            {
                transform.parent.GetComponent<PlayerMovement>().currentBreak += 10;
                GameObject.Find("Canvas").transform.GetChild(4).GetComponent<BreakBar>().breakSlider.value = transform.parent.GetComponent<PlayerMovement>().currentBreak / transform.parent.GetComponent<PlayerMovement>().maxBreak;
                transform.parent.GetComponent<PlayerMovement>().PV.RPC("UpdateBreak", RpcTarget.All, transform.parent.GetComponent<PlayerMovement>().currentBreak);
                transform.parent.GetComponent<PlayerMovement>().source2.Play();
            }
            Destroy(col.gameObject);
        }
    }
}
