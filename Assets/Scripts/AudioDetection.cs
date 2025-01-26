
    
using System.Collections.Generic;
using System;

using TMPro;
using Meta.Voice.Samples.Dictation;
using UnityEngine;
using Meta.WitAi.Lib;
//using UnityEngine.InputSystem;
using Oculus.Voice.Dictation;
using System.Globalization;
using Meta.Voice;
using Meta.WitAi;
using Meta.WitAi.Configuration;
using Meta.WitAi.Data.Configuration;
using Meta.WitAi.Dictation;
using Meta.WitAi.Dictation.Data;
using Meta.WitAi.Interfaces;
using Meta.WitAi.Requests;
using Oculus.Voice.Dictation.Bindings.Android;
using Oculus.VoiceSDK.Utilities;
using Oculus.Voice.Core.Bindings.Android.PlatformLogger;
using Oculus.Voice.Core.Bindings.Interfaces;





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

    public Microphone lol;

    public AppDictationExperience ADE;
    public WitRuntimeConfiguration Eng;
    public WitRuntimeConfiguration Spa;

    public VoiceTranslation vtss;




    private bool change;
    string m_DeviceName;


    //public InputActionReference test;
 
    private void Start() {

        /*var devices = Microphone.devices;
        foreach (var device in devices)
        {
            
            Debug.Log(device);
            m_DeviceName = device;
            break;
            
              
        }*/

        //test.action.started += StartRecording;
        //test.action.canceled += EndRecording;
        



    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A)) StartRecording();

        if (OVRInput.GetUp(OVRInput.RawButton.A)) EndRecording();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRecording();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndRecording();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SpanishesYourText();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EngishesYourText();
        }
    }

    public void StartRecording() {

        //if(!isRecording)
        //{

            Debug.Log("Recording");
            isRecording = true;
            script.ToggleActivation();
        //}
        

    }

    public void EndRecording() {

        //if(isRecording)
        //{
            Debug.Log("Stopped");
            script.ToggleActivation();
            isRecording = false;
        
            //time = 0;
            
            //Microphone.End(null);

            //return clip;

        //}

        //return null;
       
    }

    public void SpanishesYourText()
    {
        return;
    }

    public void EngishesYourText()
    {
        //ADE.RuntimeDictationConfiguration = Eng;
        return;
    }

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