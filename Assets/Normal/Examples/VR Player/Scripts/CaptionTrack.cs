using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
//using UnityEngine.InputSystem.LowLevel;


public class CaptionTrack : RealtimeComponent<CaptionModel>
{
    public CaptionComponent TranslateThing;
    public Transform headtrans;
    public float downOffset = .5f;
    Transform cameratransform;
    private float timer;
    public TextMeshPro text;
    public string userLang;
    //public string localLang = "Spanish";

    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        cameratransform = Camera.main.transform;
        UpdateServerLang("English");
        if ( isOwnedLocallyInHierarchy)
        {
         LocalLanguage.instance.localPlayerCap = this;
         userLang = LocalLanguage.instance.localLanguage;
        }
    }

    // Update is called once per frame
    void Update()
    {
      transform.position = headtrans.position + Vector3.down*downOffset;
        transform.LookAt(cameratransform.position);
        
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            timer = 0;
            //debugging func
            if(realtimeView.isOwnedLocallyInHierarchy){
                
                //text.text = Time.time.ToString("00.00");
                //UpdateServerCaption();
                
            }
        }


    }

    void UpdateLocalText(CaptionModel Model,string NewCaption)
    {
        text.text = model.captionString;
        TranslateThing.TranslateText(userLang,LocalLanguage.instance.localLanguage,model.captionString);
    }

    void UpdateLocalLang(CaptionModel Model, string NewLanguage)
    {
        userLang = Model.userLang;
    }

    protected override void OnRealtimeModelReplaced(CaptionModel previousModel, CaptionModel currentModel)
    {
        if (previousModel != null)
        {
            model.captionStringDidChange -= UpdateLocalText;
            model.userLangDidChange -= UpdateLocalLang;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.captionString = text.text;
            }
            currentModel.captionStringDidChange += UpdateLocalText;
            model.userLangDidChange += UpdateLocalLang;
        }
    }

    public void UpdateServerLang(string newLang)
    {
        model.userLang = newLang;
    }


    public void UpdateServerCaption()
    {
        model.captionString = text.text;
        
    }
}
