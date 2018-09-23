using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logs : MonoBehaviour {
    TMPro.TextMeshProUGUI[] textList;
    private int listIndex;
    Queue<string> queue;
	// Use this for initialization
	void Start () {
        textList = GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
        print(textList.Length);

        queue = new Queue<string>();
        StartCoroutine(LogsCor());
        queue.Enqueue("PlayerX shot PlayerY");
        queue.Enqueue("PlayerX Disconnected");
        queue.Enqueue("PlayerX wants to go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> killed <#4775FF>Player Y</color>");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go right");
        queue.Enqueue("<#ffff80>PlayerX</color> go right");
        queue.Enqueue("<#ffff80>PlayerX</color> go left");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go down");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go up");
        queue.Enqueue("<#ffff80>PlayerX</color> go Left");
        queue.Enqueue(GetColoredText("PlayerX", Color.cyan)+" go up");

    }
	
	public void SendLog(string log)
    {
        queue.Enqueue(log);
    }
    public static string GetColoredText(string text, Color color)
    {
        string colString = ColorUtility.ToHtmlStringRGB(color);
        return "<#" + colString + ">" + text + "</color>";
    }

    private TMPro.TextMeshProUGUI getAText()
    {
        var toRet = textList[listIndex];
        listIndex = ++listIndex % 10;
        return toRet;
    }
    
    IEnumerator LogsCor()
    {
        string log = null;
        while(true)
        {
            if (queue.Count != 0)
            {
                log = queue.Dequeue();
                foreach (var t in textList)
                {
                    t.rectTransform.localPosition = new Vector3(t.rectTransform.localPosition.x, t.rectTransform.localPosition.y + 15);
                }
                
                var container = getAText();
                container.rectTransform.localPosition = new Vector3(container.rectTransform.localPosition.x, -95);
                container.text = "";
                container.gameObject.SetActive(true);
                for (int i = 0; i < log.Length; ++i)
                {
                    if (log[i] == '<')
                    {
                        string tag = "";
                        for (; log[i] != '>' && i < log.Length; ++i)
                        {
                            tag += log[i];
                        }
                        container.text += tag + log[i];
                    }
                    else
                        container.text += log[i];
                    yield return new WaitForSeconds(0.01f);
                }
            }
            yield return null;
        }
    }
}
