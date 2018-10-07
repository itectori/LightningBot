using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsAnimator : MonoBehaviour {
    RawImage[] children;
    Text[] childrenText;
    Vector3[] startPoses;
    Vector3[] endPoses;
    bool deployed = false;
    int numberOfDeployedItem = 0;
    [HideInInspector]
    public string[] changeWhenPossible = null;
    [HideInInspector]
    public string[] currentGameTab = null;


	// Use this for initialization
	void Start () {
        children = transform.GetComponentsInChildren<RawImage>();
        childrenText = transform.GetComponentsInChildren<Text>();
        startPoses = new Vector3[children.Length];
        endPoses = new Vector3[children.Length];
        for (int i = 0; i < children.Length; ++i)
        {
            startPoses[i] = children[i].rectTransform.position;
            endPoses[i] = startPoses[i] + new Vector3(182, 0, 0);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowItem(int num, float delay=0)
    {
        StartCoroutine(Deploy(num, 0));
    }
    public void HideAllItems()
    {
        StartCoroutine(Undeploy());
    }

    public void SetChildrenText(string[] gameTab)
    {
        if (deployed)
            changeWhenPossible = gameTab;
        else
        {
            for (int i = 0; i < gameTab.Length && i < 15; ++i)
                childrenText[i].text = gameTab[i];
            currentGameTab = gameTab;
        }
    }
    
    IEnumerator Deploy(int num, float wait)
    {
        while (deployed) yield return null;
        if (changeWhenPossible.Length != 0)
        {
            SetChildrenText(changeWhenPossible);
        }
        if (num > 15)
            num = 15;
        numberOfDeployedItem = num;
        yield return new WaitForSeconds(wait);
        float[] indexTab = new float[num];
        int nbItem = 1;
        
        while (indexTab[indexTab.Length-1] < 1)
        {
            for (var i = 0; i < nbItem; ++i)
            {
                indexTab[i] += Time.deltaTime * 4;
                children[i].rectTransform.position = Vector3.Lerp(startPoses[i], endPoses[i], Mathf.SmoothStep(0, 1, indexTab[i]));
            }
            if (indexTab[nbItem - 1] > 0.15f && nbItem < num)
                nbItem += 1;
            yield return null;
        }
        deployed = true;
    }
    IEnumerator Undeploy(float wait = 0)
    {

        yield return new WaitForSeconds(wait);
        float[] indexTab = new float[numberOfDeployedItem];
        int nbItem = 1;
        while (indexTab[indexTab.Length - 1] < 1)
        {
            for (var i = 0; i < nbItem; ++i)
            {
               
                indexTab[i] += Time.deltaTime * 4;
                if (children[i].rectTransform.position == startPoses[i])
                    continue;
                children[i].rectTransform.position = Vector3.Lerp(endPoses[i], startPoses[i], Mathf.SmoothStep(0, 1, indexTab[i]));
            }
            if (indexTab[nbItem - 1] > 0.15f && nbItem < indexTab.Length)
                nbItem += 1;
            yield return null;
        }
        deployed = false;
    }
}
