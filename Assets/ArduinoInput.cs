using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoInput : MonoBehaviour {
    [SerializeField]
    string usedPort = "COM5";
    SerialPort stream;
    string lastEvent = "";
    string prevEvent = "";

    void Start () {
        stream = new SerialPort(usedPort, 9600);
        stream.ReadTimeout = 50;
        stream.Open();
        
        StartCoroutine(AsynchronousReadFromArduino((string s) => deb(s) ));
    }

    void deb(string s) {
        lastEvent = s;
    }

    void Update() {
        if(prevEvent != lastEvent) {
            Debug.Log(lastEvent);
        }
        prevEvent = lastEvent;
    }
	
    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity) {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do {
            try {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException) {
                dataString = null;
            }

            if (dataString != null) {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    void OnDestroy() {
        // Close the port when the program ends.
        if (stream.IsOpen) {
            try {
                stream.Close();
            }
            catch (UnityException e) {
                Debug.LogError(e.Message);
            }
        }
    }
}
