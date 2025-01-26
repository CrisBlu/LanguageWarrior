using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class CaptionTrack : RealtimeComponent<CaptionModel>
{
    public Transform headtrans;
    public float downOffset = .5f;
    Transform cameratransform;
    private float timer;
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        cameratransform = Camera.main.transform;
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
    }

    protected override void OnRealtimeModelReplaced(CaptionModel previousModel, CaptionModel currentModel)
    {
        if (previousModel != null)
        {
            model.captionStringDidChange -= UpdateLocalText;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.captionString = text.text;
            }
            currentModel.captionStringDidChange += UpdateLocalText;
        }
    }

    public void UpdateServerCaption()
    {
        model.captionString = text.text;
        
    }
}
