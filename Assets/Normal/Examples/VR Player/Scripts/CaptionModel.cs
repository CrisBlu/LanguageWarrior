using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RealtimeModel]
public partial class CaptionModel
{
    [RealtimeProperty(2, false, true)]
    private string _userLang;
    [RealtimeProperty(1, false, true)]
    private string _captionString;
   
}

