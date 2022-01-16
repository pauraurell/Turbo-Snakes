using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using System;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]

public class Tail : MonoBehaviour
{
    public float pointSpacing = 0.1f;
    public GameObject head;
    Color[] colors = new Color[8];
    Vector2[] a = new Vector2[2];
    private PhotonView PV;

    public List<Vector2> points;
    public LineRenderer line;
    public EdgeCollider2D col;

    public int lineColor;
    private int newColor;
    int framesToDeletePoint = 50;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        line = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();

        colors[0] = Color.red;
        colors[1] = Color.cyan;
        colors[2] = Color.green;
        colors[3] = Color.grey;
        colors[4] = Color.blue;
        colors[5] = Color.magenta;
        colors[6] = Color.yellow;
        colors[7] = Color.white;

        if (PV.IsMine)
        {
            newColor = UnityEngine.Random.Range(0, 8);
            PV.RPC("AssignColors", RpcTarget.All, newColor);
        }

        points = new List<Vector2>();

        col.offset = new Vector2(-this.transform.parent.GetChild(1).transform.position.x, -this.transform.parent.GetChild(1).transform.position.y);

        a[0] = new Vector2(transform.position.x, transform.position.y);
        a[1] = new Vector2(transform.position.x, transform.position.y);
        col.points = a;
        SetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(points.Last(), head.transform.position) > pointSpacing)
        {
            SetPoint();
        }

        line.startColor = colors[lineColor];
        line.endColor = line.startColor;


        /*if (Time.frameCount % framesToDeletePoint == 0)
        {
            int newPositionCount = line.positionCount - 1;
            Vector3[] newPositions = new Vector3[newPositionCount];

            for (int i = 0; i < newPositionCount; i++)
            {
                newPositions[i] = line.GetPosition(i + 1);
            }

            line.SetPositions(newPositions);

            if (framesToDeletePoint % 5 == 0 || framesToDeletePoint % 4 == 1)
            {
                List<Vector2> newPoints = new List<Vector2>(col.points);
                newPoints.RemoveAt(0);
                col.points = newPoints.ToArray<Vector2>();
            }



            //col.pointCount--;

            //RemoveElement(ref col.points, 1);

            framesToDeletePoint++;
        }*/


    }


    void SetPoint() 
    {
        if (points.Count > 1)
        {
            List<Vector2> tmpList = col.points.ToList<Vector2>();
            Vector2 vec = head.transform.position;
           // vec.
            tmpList.Add(head.transform.position);
            col.points = tmpList.ToArray();
        }

        points.Add(head.transform.position);

        line.positionCount = points.Count;

        line.SetPosition(points.Count - 1, head.transform.position);

    }



    [PunRPC]
    public void AssignColors(int c) 
    {
        lineColor = c;
    }

    private void RemoveElement<T>(ref T[] arr, int index) 
    {
        for (int i = index; i < arr.Length-1; i++)
        {
            arr[i] = arr[i + 1];
        }
        Array.Resize(ref arr, arr.Length-1);
    }
}
