/*
Unity and Arduino serial port communication example

* Unity read + Arduino send 只需 activate SerialReceiver script
* Unity send + Arduino read 除了需要 SerialSenders script 也要 SerialReceiver 供Arduino回傳時接收檢查

*/
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour {
    [SpaceAttribute(10)]
    [HeaderAttribute("                       ------- Port Settings ------- ")]
    [SpaceAttribute(10)]
    public string Port = "COM3"; // Replace with your Arduino port
    public int BaudRate = 9600; // Baud rate for serial communication

    [HideInInspector]
    public SerialPort serialPort;

    void Awake()
    {
        InitializeSerialPort();
    }

    private void InitializeSerialPort()
    {
        serialPort = new SerialPort(Port, BaudRate)
        {
            ReadTimeout = 50, // Timeout for reading serial input
            WriteTimeout = 50 // Timeout for writing serial output
        };

        try
        {
            serialPort.Open();
            Debug.Log($"Serial port {Port} opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to open serial port: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
