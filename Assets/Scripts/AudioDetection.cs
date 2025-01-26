
    
using System.Collections.Generic;
using System;

using TMPro;
using Meta.Voice.Samples.Dictation;
using UnityEngine;
//using UnityEngine.InputSystem;




public class MicrophoneManager : MonoBehaviour {
    // Display
    //[SerializeField] private TMP_Text inputBox;
    //[SerializeField] private TMP_Text outputBox;
    //[SerializeField] private UnityEngine.UI.Image symbol;
   
    public TMP_Text text;



    // Whisper
    private readonly string fileName = "output.wav";

    private readonly int MAX_DURATION = 30;
    public AudioClip clip;
    public bool isRecording;
    private float time = 0;

    public AudioSource audioSource;

    public DictationActivation script;




    private bool change;
    string m_DeviceName;


    //public InputActionReference test;
 
    private void Start() {
        

        var devices = Microphone.devices;
        foreach (var device in devices)
        {
            
            Debug.Log(device);
            m_DeviceName = device;
            break;
            
              
        }

        //test.action.started += StartRecording;
        //test.action.canceled += EndRecording;




    }

    void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            Debug.Log("recording");
            
            //StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("end");
            
            //runWhisper.StartProcess(clip, this);
            //audioSource.PlayOneShot(clip);
        }*/
    }

    //public void StartRecording(InputAction.CallbackContext context) {

        //if(!isRecording)
        //{
            //Debug.Log("Recording");
            //script.ToggleActivation();
            //isRecording = true;
        

        
            //clip = Microphone.Start(m_DeviceName, false, MAX_DURATION, 16000);

        //}

    //}
    //public void EndRecording(InputAction.CallbackContext context) {

        //if(isRecording)
        //{
            //Debug.Log("Stopped");
            //script.ToggleActivation();
            //isRecording = false;
        
            //time = 0;
            
            //Microphone.End(null);

            //return clip;

        //}

        //return null;
       
    //}

    /*void lol(InputAction.CallbackContext context)
    {
        Debug.Log("hello");
    }

    void ool(InputAction.CallbackContext context)
    {
        Debug.Log("goodbye");
    }*/

    // Update is called once per frame
    
    
    



}