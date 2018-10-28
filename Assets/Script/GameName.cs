using UnityEngine;

public class GameName : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI tm;

    private void Start()
    {
        tm = GetComponent<TMPro.TextMeshProUGUI>();
        tm.text = "";
    }

    public static void SetGameName(string name)
    {
        if (name[0] == '_')
            name = name.Remove(0, 1);
        tm.text = name;
    }
}