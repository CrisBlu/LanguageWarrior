using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLanguage : MonoBehaviour
{
    public static LocalLanguage instance;
    public CaptionTrack localPlayerCap;
public string localLanguage = "Spanish";
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void changeLanguage(string newLanguage)
    { 
        Debug.Log("change lang func" + newLanguage);
        localLanguage = newLanguage;
        if (localPlayerCap == null)
        {
            localPlayerCap.userLang = newLanguage;
            localPlayerCap.UpdateServerLang(newLanguage);
        }
        else
        {
            Debug.Log("local player is not here");
        }
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
