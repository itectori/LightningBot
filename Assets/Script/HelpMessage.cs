using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMessage : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI tm;
    
    private void Start()
    {
        tm = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public static void DeleteMessage()
    {
        tm.text = "";
    }
}