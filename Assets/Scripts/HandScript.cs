using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class HandScript : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 1234;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;
    Vector3 worldPos;
    Animator animator;

    private Camera cam;

    bool running;

    public bool closed;

    public float positionThreshold = 2f;
    public float resetTime = 2f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    private void Update()
    {
        float handIsClosed = receivedPos.x;

        Vector3 targetPosition = cam.ScreenToWorldPoint(new Vector3(receivedPos.y, receivedPos.z, 5));

        if (Vector3.Distance(worldPos, targetPosition) < positionThreshold)
        {
            worldPos = targetPosition;
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
            
            if (timer >= resetTime)
            {
                worldPos = targetPosition;
                timer = 0f; 
            }
        }

        transform.position = worldPos;

        if (handIsClosed == 1f)
        {
            animator.SetBool("isOpen", false);
            closed = true;
        }
        else
        {
            animator.SetBool("isOpen", true);
            closed = false;
        }
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        Vector3 point = new Vector3();

        if (dataReceived != null)
        {
            string[] handState = dataReceived.Split(' ');

            float handClosed = float.Parse(handState[0]);

            float relXPos = 1f - float.Parse(handState[1]);
            float relYPos = 1f - float.Parse(handState[2]);

            float yPos = relYPos * Screen.height;
            float xPos = relXPos * Screen.width;

            Debug.Log(yPos.ToString() + " " + xPos.ToString());

            receivedPos = new Vector3(handClosed, xPos, yPos);
        }
    }

    void OnDestroy()
    {
        listener.Stop();
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

}
