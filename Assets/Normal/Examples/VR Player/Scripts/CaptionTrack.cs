using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class CaptionTrack : MonoBehaviour
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
            text.text = Time.time.ToString();
        }


    }

    private void TextChange()
    {
        text.text = "Caption has Changed";
        

    }
}
