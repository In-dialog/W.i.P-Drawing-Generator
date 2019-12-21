using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;//.Ports;


public class SendToArduino : MonoBehaviour
{
    SerialController serialController;
    public List<string> serial_ports = new List<string>();
    public List<string> _messages = new List<string>();
    public string serialPort;
    public string message;

    DrawSimbol drawSimbol;
    public List<string> positionsToSend = new List<string>();
    public List<Vector3> positions = new List<Vector3>();


    double timeT;
    void Start()
    {
        drawSimbol = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<DrawSimbol>();
        serialController = this.GetComponent<SerialController>();
        getPortNames();
        if (serial_ports.Count - 1 == 0)
        {
            serialPort = serial_ports[0];
            ActivateConection();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))positions.Clear();
        ///////// get coordonates from Pen
        timeT += Time.deltaTime;
        if (drawSimbol.linearMovment & drawSimbol.bestTarget!=null)
        {
            if (!positions.Contains(drawSimbol.bestTarget.position))
            {
                positions.Add(drawSimbol.bestTarget.position);
                positionsToSend.Add("G00X" + Mathf.Round(drawSimbol.bestTarget.position.x).ToString() + "Y" + Mathf.Round(drawSimbol.bestTarget.position.z).ToString());
            }
        }
        else
        {
            if (timeT > 0.2f)
            {
                //positions.Add(drawSimbol.transform.position);
                timeT = 0;
            }
        }
        ///////// compose string
        ReciveData();

        if (_messages.Count > 0)
        {
            if (_messages[0].Contains("Grbl"))
            {
                Debug.Log("!!!!!!!");
                serialController.SendSerialMessage("G00X00Y00");
                _messages.RemoveAt(0);
            }
            else
            {
                if (_messages[0].Contains("ok") & positionsToSend.Count > 0)
                {
                    //string toSend = "G00X" + Mathf.Round(positions[0].x).ToString() + "Y" + Mathf.Round(positions[0].z).ToString();
                    serialController.SendSerialMessage(positionsToSend[0]);
                    //positions.RemoveAt(0);
                    positionsToSend.RemoveAt(0);
                    _messages.RemoveAt(0);
                }
            }
        }
    }
    void ActivateConection()
    {
        serialController.portName = serialPort;
        serialController.enabled = true;
    }
    void getPortNames()
    {
        int p = (int)System.Environment.OSVersion.Platform;
        // Are we on Unix?
        if (p == 4 || p == 128 || p == 6)
        {
            string[] ttys = System.IO.Directory.GetFiles("/dev/", "tty.*");
            foreach (string dev in ttys)
            {
                //if(ttys.Contains())
                if (dev.StartsWith("/dev/tty.usbmodem"))
                    serial_ports.Add(dev);
                else

                    Debug.LogWarning(System.String.Format(dev));
            }
        }
    }
    void ReciveData()
    {
        message = serialController.ReadSerialMessage();
        if (message == null)
            return;
        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            Debug.LogWarning("Message arrived: " + message);
            if (message.Length > 1)
                _messages.Add(message);
        }
    }
}