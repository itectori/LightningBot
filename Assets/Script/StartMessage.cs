using UnityEngine;

public class StartMessage : MonoBehaviour
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