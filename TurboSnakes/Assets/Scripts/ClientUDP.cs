using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class Packet
{
    public List<PlayerInput> inputs = new List<PlayerInput>();
    public List<Objects> objects = new List<Objects>();
}

[System.Serializable]
public class FirstPacket
{
    public string guid1, guid2;
    public List<Objects> objects = new List<Objects>();
}

[System.Serializable]
public class PlayerInput
{
    public string key;
    public string type;
}

[System.Serializable]
public class Objects
{
    public string name;
    private GameObject go;
    public Vector3 position;
    public string uid;

    public GameObject getGameObject()
    {
        return go;
    }

    public void setGameObject(GameObject gameobject)
    {
        go = gameobject;
    }
}

public class ClientUDP : MonoBehaviour
{

    Socket socket;
    IPEndPoint ipep;
    IPEndPoint sender;
    EndPoint remote;
    Thread emitter;
    Thread listener;
    Thread mainThread;

    public GameObject text;

    // public Text text;
    // bool printText = false;

    private List<PlayerInput> inputList = new List<PlayerInput>();
    public List<Objects> updateList = new List<Objects>();

    public Dictionary<string, GameObject> DynamicObjects = new Dictionary<string, GameObject>();

    public GameObject[] first_objects;

    // Start is called before the first frame update
    void Start()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

        socket = new Socket(endPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        ipep = endPoint;

        mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();

        emitter = new Thread(new ThreadStart(Send));
        listener = new Thread(new ThreadStart(Listen));

    }

    // Update is called once per frame
    void Update()
    {
        if (updateList.Count > 0)
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                if (updateList[i] != null)
                {

                    GameObject update_object = DynamicObjects[updateList[i].uid];
                    if (update_object != null)
                    {
                        //Debug.Log(update_object.name);
                        update_object.GetComponent<Transform>().position = updateList[i].position;
                    }
                    else
                    {
                        Debug.LogWarning("Can't find object by name" + updateList[i].name);
                    }
                }
            }
            updateList.Clear();
        }
    }

    void MainThread()
    {
        try
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes("PING");
            socket.SendTo(msg, msg.Length, SocketFlags.None, ipep);

            sender = new IPEndPoint(IPAddress.Any, 0);
            remote = (EndPoint)sender;

            Debug.Log("Message sent");

            msg = new Byte[10000];
            socket.ReceiveFrom(msg, ref remote);

            MemoryStream stream = new MemoryStream(msg);
            FirstPacket packet = deserializeJsonFirst(stream);

            DynamicObjects.Add(packet.guid1, first_objects[0]);
            DynamicObjects.Add(packet.guid2, first_objects[1]);

            foreach (Objects obj in packet.objects)
            {
                updateList.Add(obj);
            }
        }

        catch (SystemException e)
        {
            Debug.Log("Couldn't recieve the message");
            Debug.Log(e.ToString());
            Debug.Log("Disconnecting from server");
            mainThread.Abort();
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SystemException d)
            {
                Debug.Log("Socket closed");
                Debug.Log(d.ToString());
            }
        }
        emitter.Start();
        listener.Start();

    }

    void Listen()
    {
        while (true)
        {
            try
            {
                byte[] msg = new byte[10000];
                socket.ReceiveFrom(msg, ref remote);

                MemoryStream stream = new MemoryStream(msg);
                Packet p = deserializeJson(stream);

                if (p != null)
                {
                    //Debug.Log(p.objects.Count);
                    foreach (Objects o in p.objects)
                    {
                        updateList.Add(o);
                    }
                }
               
            }
            catch (SystemException e)
            {
                Debug.LogException(e);
            }

        }
    }

    void Send()
    {
        while (true)
        {
            if (inputList.Count > 0)
            {
                Packet temp = new Packet();
                MemoryStream stream = new MemoryStream();
               
                temp.inputs = inputList;
                stream = serializeJson(temp);

                socket.SendTo(stream.ToArray(), SocketFlags.None, ipep);
            }
        }
    }

    public void AddInput(KeyCode input, string type)
    {
        PlayerInput newInput = new PlayerInput();
        newInput.key = input.ToString();
        newInput.type = type;
        inputList.Add(newInput);
    }

    MemoryStream serializeJson(Packet p)
    {
        string json = JsonUtility.ToJson(p);

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(json);

        inputList.Clear();
        return stream;
    }

    Packet deserializeJson(MemoryStream stream)
    {
        var p = new Packet();

        BinaryReader reader = new BinaryReader(stream);
        stream.Seek(0, SeekOrigin.Begin);

        string json = reader.ReadString();

        p = JsonUtility.FromJson<Packet>(json);
        return p;
    }

    FirstPacket deserializeJsonFirst(MemoryStream stream)
    {
        var p = new FirstPacket();
        BinaryReader reader = new BinaryReader(stream);

        stream.Seek(0, SeekOrigin.Begin);
        string json = reader.ReadString();

        //Debug.Log(json);
        p = JsonUtility.FromJson<FirstPacket>(json);
        return p;
    }

    public void EndConnection()
    {
        Debug.Log("Disconnecting from server");
        mainThread.Abort();

        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        catch (SystemException e)
        {
            Debug.Log("Socket closed");
            Debug.Log(e.ToString());

        }

        if (mainThread != null)
            mainThread.Abort();
        if (listener != null)
            listener.Abort();
        if (emitter != null)
            emitter.Abort();
    }

    void OnApplicationQuit()
    {
        try
        {
            socket.Close();

        }

        catch (SystemException e)
        {
            Debug.Log("Socket closed");
            Debug.Log(e.ToString());

        }
        if (mainThread != null)
            mainThread.Abort();
        if (listener != null)
            listener.Abort();
        if (emitter != null)
            emitter.Abort();
    }

}