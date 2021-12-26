using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;


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
    private GameObject gameobject;
    public Vector3 position;
    public string guid;

    public GameObject getGO()
    {
        return gameobject;
    }

    public void setGO(GameObject go)
    {
        gameobject = go;
    }
}

public class ServerUDP : MonoBehaviour
{
    IPEndPoint ipep;
    IPEndPoint sender;
    IPEndPoint sender2;
    EndPoint remote;
    EndPoint remote2;

    Thread connection;
    Thread connection2;
    Thread listener;
    Thread listener2;
    Thread send;

    private List<PlayerInput> inputList = new List<PlayerInput>();
    public List<Objects> updateList = new List<Objects>();

    snake snake1;
    snake snake2;

    Packet p = new Packet();

    class Client
    {
        public Socket socket;
        public EndPoint remote;
        public bool active = false;
    }

    public List<Objects> DynamicGameObjects = new List<Objects>();
    public GameObject player1;
    public GameObject player2;
    public bool player1connected;
    public bool player2connected;

    Client client1 = new Client();
    Client client2 = new Client();

    public GameObject[] SceneGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        snake1 = player1.GetComponent<snake>();
        snake2 = player2.GetComponent<snake>();

        player1connected = false;
        player2connected = false;

        foreach (GameObject go in SceneGameObjects)
        {
            if(go.name != "Variables Saver")
            {
                Objects newSceneObject = new Objects();
                newSceneObject.setGO(go);
                newSceneObject.name = go.name;
                newSceneObject.position = go.transform.position;
                newSceneObject.guid = Guid.NewGuid().ToString();
                DynamicGameObjects.Add(newSceneObject);
            }
        }


        ipep = new IPEndPoint(IPAddress.Any, 7777);

        client1.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        client1.socket.Bind((EndPoint)ipep);

        ipep = new IPEndPoint(IPAddress.Any, 7778);
        client2.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        client2.socket.Bind((EndPoint)ipep);

        sender = new IPEndPoint(IPAddress.Any, 0);
        sender2 = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)(sender);
        remote2 = (EndPoint)(sender2);

        connection = new Thread(new ThreadStart(Connection));
        connection.Start();
        connection2 = new Thread(new ThreadStart(Connection2));
        connection2.Start();
        listener = new Thread(new ThreadStart(Listen));
        listener.Start();
        listener2 = new Thread(new ThreadStart(Listen2));
        listener2.Start();
        send = new Thread(new ThreadStart(Send));
        send.Start();
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i < updateList.Count; i++)
        {
            for (int j = 0; j < DynamicGameObjects.Count; j++)
            {
                if(updateList[i].guid == DynamicGameObjects[j].guid)
                {
                    GameObject update_object = DynamicGameObjects[j].getGO();
                    update_object.transform.rotation = updateList[i].rota
                }
            }
        }*/

    }

    void Connection()
    {
        while (true)
        {
            if (!client1.active)
            {
                try
                {
                    byte[] msg = new Byte[256];
                    int recv = client1.socket.ReceiveFrom(msg, ref remote);
                    client1.active = true;
                    client1.remote = remote;
                    

                    FirstPacket packet = new FirstPacket();
                    packet.guid1 = DynamicGameObjects[0].guid;
                    packet.guid2 = DynamicGameObjects[1].guid;

                    packet.objects = DynamicGameObjects;
                    MemoryStream stream = new MemoryStream();
                    stream = serializeJsonFirst(packet);
                    client1.socket.SendTo(stream.ToArray(), SocketFlags.None, remote);

                    player1connected = true;
                }
                catch (SystemException e)
                {

                    Debug.Log("Cause of death: Could not send or receive message");
                    Debug.Log(e.ToString());
                }
            }
        }
    }

    void Connection2()
    {
        while (true)
        {
            if (!client2.active)
            {
                try
                {
                    byte[] msg = new Byte[256];
                    int recv = client2.socket.ReceiveFrom(msg, ref remote2);
                    client2.active = true;
                    client2.remote = remote2;

                    FirstPacket packet = new FirstPacket();
                    packet.guid1 = DynamicGameObjects[0].guid;
                    packet.guid2 = DynamicGameObjects[1].guid;

                    packet.objects = DynamicGameObjects;
                    MemoryStream stream = new MemoryStream();
                    stream = serializeJsonFirst(packet);
                    client2.socket.SendTo(stream.ToArray(), SocketFlags.None, remote2);

                    player2connected = true;
                }
                catch (SystemException e)
                {

                    Debug.Log("Cause of death: Could not send or receive message");
                    Debug.Log(e.ToString());
                }
            }
        }
    }

    void Listen()
    {
        while (true) 
        {
            if (client1.active) 
            {
                try
                {
                    byte[] msg = new Byte[2000];

                    int recv = client1.socket.ReceiveFrom(msg, ref client1.remote);

                    MemoryStream stream = new MemoryStream(msg);

                    Packet packet = deserializeJson(stream);

                    foreach(PlayerInput input in packet.inputs)
                    {
                        snake1.AddInput(input);
                        //Debug.Log(input.key);
                    }

                    foreach (Objects obj in packet.objects)
                    {
                        updateList.Add(obj);
                    }

                }
                catch(SystemException e)
                {
                    Debug.Log("Cause of death: Couldn't recieve message");
                    Debug.Log(e.ToString());
                    client1.active = false;
                }
            }
            
        }
        
    }

    void Listen2()
    {
        while (true)
        {
            if (client2.active)
            {
                try
                {
                    byte[] msg = new Byte[2000];

                    int recv = client2.socket.ReceiveFrom(msg, ref client2.remote);

                    MemoryStream stream = new MemoryStream(msg);

                    Packet packet = deserializeJson(stream);

                    foreach (PlayerInput input in packet.inputs)
                    {
                        snake2.AddInput(input);
                    }

                    foreach (Objects obj in packet.objects)
                    {
                        updateList.Add(obj);
                    }

                }
                catch (SystemException e)
                {
                    Debug.Log("Cause of death: Couldn't recieve message");
                    Debug.Log(e.ToString());
                    client2.active = false;
                }
            }
        }

    }

    void Send()
    {
        
        while (true)
        {
            //byte[] bytes = System.Text.Encoding.ASCII.GetBytes("Received: pong");
            //int bytesSent = socket.SendTo(bytes, bytes.Length, SocketFlags.None, ipep);

            List<Objects> UpdatedGameObjects = new List<Objects>();
            
            foreach(Objects go in DynamicGameObjects)
            {
                UpdatedGameObjects.Add(go); 
            }

            if(UpdatedGameObjects.Count > 0)
            {
                p.objects = UpdatedGameObjects;
            }

            if (client1.active)
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    stream = serializeJson(p);
                    
                    
                    client1.socket.SendTo(stream.ToArray(), SocketFlags.None, remote);
                    

                }
                catch(SystemException e)
                {
                    Debug.Log("Cause of Death: Could not send message");
                    Debug.Log(e.ToString());
                    client1.active = false;
                }
            }

            if (client2.active)
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    stream = serializeJson(p);
                    client2.socket.SendTo(stream.ToArray(), SocketFlags.None, remote);
                    

                }
                catch (SystemException e)
                {
                    Debug.Log("Cause of Death: Could not send message");
                    Debug.Log(e.ToString());
                    client2.active = false;
                }
            }

            p.inputs.Clear();
            p.objects.Clear();
        }


        
    }

    public void EndConnection()
    {
        Debug.Log("Cause of death: Stopping UDP Server connection");
        try
        {
            // newSocket.Shutdown(SocketShutdown.Both);
            client1.socket.Close();
            client2.socket.Close();
        }
        catch (SystemException e)
        {
            Debug.Log("Cause of death: Socket already closed");
            Debug.Log(e.ToString());

        }
        if (connection != null)
            connection.Abort();
        if (connection2 != null)
            connection2.Abort();
        if (listener != null)
            listener.Abort();
        if (listener2 != null)
            listener2.Abort();
        if (send != null)
            send.Abort();

    }

    void OnApplicationQuit()
    {
        try
        {
            client1.socket.Shutdown(SocketShutdown.Both);
            client1.socket.Close();
            client2.socket.Shutdown(SocketShutdown.Both);
            client2.socket.Close();
        }
        catch (SystemException e)
        {
            Debug.Log("Cause of death: Socket already closed");
            Debug.Log(e.ToString());

        }

        if (connection != null)
            connection.Abort();
        if (connection2 != null)
            connection2.Abort();
        if (listener != null)
            listener.Abort();
        if (listener2 != null)
            listener2.Abort();
        if (send != null)
            send.Abort();
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

    MemoryStream serializeJsonFirst(FirstPacket p)
    {
        string json = JsonUtility.ToJson(p);
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(json);

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


}
