/*
Unity send + Arduino read example

* 因為Unity send 時不能開Arduino serial port monitor檢查，所以會回傳給unity供檢查，這時Unity的receive script的buffer size也要記得設定成n+1 

*/
using System;
using System.Text;  
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Collections;

public class SerialSender : MonoBehaviour {
    private SerialManager serialManager;

    private void Awake () {
        serialManager = GetComponent<SerialManager>();
    }

    public new void SendMessage(string message)
    {
        if (serialManager.serialPort != null && serialManager.serialPort.IsOpen)
        {
            try
            {
                serialManager.serialPort.WriteLine(message); // Send the message
                Debug.Log($"Sent message: {message}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to send message: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Serial port is not open.");
        }
    }
}
