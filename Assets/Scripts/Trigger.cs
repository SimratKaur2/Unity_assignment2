using System;
using System.Threading;
using UnityEngine;
using NetworkAPI;

public class Trigger : MonoBehaviour
{
    NetworkComm networkComm;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        networkComm.sendMessage("Another object entered trigger");
        Destroy(other.gameObject);
    }

    private void processMsg(string message)
    {
        Debug.Log("From Delegate: " + message);
    }

    void Start()
    {
        Debug.Log("Trigger Start method called.");
        networkComm = new NetworkComm();
        networkComm.MsgReceived += processMsg; // Simplified delegate attachment

        try
        {
            Debug.Log("Attempting to start receive message thread.");
            var receiveThread = new Thread(networkComm.ReceiveMessages);
            receiveThread.IsBackground = true; // Ensures thread does not prevent application from exiting
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Exception when starting the thread: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}