using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private bool move = true;
    public GameObject body;
    private bool timer = false;
    private float time = 0;
    SerialPort serialPort;
    bool turn = false;
    float delay = 0;
    bool delayStart = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        serialPort = new SerialPort("COM4",115200);
        serialPort.Open();
        Task.Run(()=>SerialThread());
    }


    async void SerialThread()
    {
        while (true)
        {
            SerialRead();
        }
    }
    void SerialRead()
    {
        if (serialPort.IsOpen)
        {
            string value = serialPort.ReadLine();
            Debug.Log(value);
            if (value == "1" &&!turn&& !delayStart)
            {
                turn = true;
            }
            else
            {
                turn = false;
            }
        }
        
    }
    void Update()
    {

        if (timer)
        {
            time+=Time.deltaTime;
        }

        if (move)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (turn && !delayStart)
        {
            animator.SetTrigger("turnBack");
            move = false;
            timer = true;
            turn = false;
            delayStart = true;
        }
        

        if (time>=6.4f)
        {
            time = 0;
            move = true;
            timer = false;
            turn = false;
            delayStart = true;
            
        }
        if (delayStart)
        {
            delay += Time.deltaTime;
        }

        if (delay > 10)
        {
            delayStart = false;
            delay = 0;
        }

        

    }

    private void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
