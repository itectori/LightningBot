using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemsAnimator : MonoBehaviour {
    RawImage[] panels;
    TMPro.TextMeshProUGUI[] childrenText;
    Vector3[] startPoses;
    Vector3[] endPoses;
    bool deployed = false;
    int numberOfDeployedItem = 0;
    int targetIndex = 0;
    [HideInInspector]
    public string[] currentGameTab = null;
    private IEnumerator deployCor;
    private IEnumerator UndeployCor;

    // Use this for initialization
    void Start () {
        panels = transform.GetComponentsInChildren<RawImage>();
        childrenText = transform.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        startPoses = new Vector3[panels.Length];
        endPoses = new Vector3[panels.Length];
        for (int i = 0; i < panels.Length; ++i)
        {
            startPoses[i] = panels[i].rectTransform.position;
            endPoses[i] = startPoses[i] + new Vector3(182, 0, 0);
        }
        
	}
	

    public void ShowItem(int num, float delay=0)
    {
        StopAllCoroutines();
        targetIndex = num -1;
        StartCoroutine(MoveRight(0));
    }
    public void FilterItem(string[] gameTab)
    {
        
        SetChildrenText(gameTab);
        int newTargetIndex = gameTab.Length-1;
        StopAllCoroutines();
        targetIndex = newTargetIndex;
        StartCoroutine(MoveLeft(14));
        StartCoroutine(MoveRight(0));
     }
    public void HideAllItems()
    {
        StopAllCoroutines();
        targetIndex = -1;
        StartCoroutine(MoveLeft(14));
    }

    public void SetChildrenText(string[] gameTab)
    {
        for (int i = 0; i < 15; ++i)
        {
            if (i >= gameTab.Length)
                childrenText[i].text = "";
            else
            {
                var newName = gameTab[i];
                if (gameTab[i][0] == '_')
                {
                    childrenText[i].text = gameTab[i].Substring(1);
                }
                else
                {
                    childrenText[i].text = gameTab[i];
                }
            }
        }
        currentGameTab = gameTab;
    }
    private float inverseLerpRight(int iPanel)
    {
        return Mathf.InverseLerp(startPoses[iPanel].x, endPoses[iPanel].x, panels[iPanel].rectTransform.position.x);
    }
    private float inverseLerpLeft(int iPanel)
    {
        return Mathf.InverseLerp(endPoses[iPanel].x, startPoses[iPanel].x, panels[iPanel].rectTransform.position.x);
       
    }

    void callNext(int iPanel)
    {
        print(iPanel + "," + targetIndex);
        if (iPanel < targetIndex)
            StartCoroutine(MoveRight(iPanel + 1));
        else if (iPanel > targetIndex)
            StartCoroutine(MoveLeft(iPanel + 1));
    }

    IEnumerator MoveRight(int iPanel)
    {
        if (iPanel > targetIndex)
            yield return null;
        else
        {
            float i = inverseLerpRight(iPanel);
            bool calledNext = false;
            while (i < 1)
            {
                if (!calledNext && i > 0.15f)
                {
                    calledNext = true;
                    if (iPanel < targetIndex)
                        StartCoroutine(MoveRight(iPanel + 1));
                }
                
                i += Time.deltaTime * 5;
                panels[iPanel].rectTransform.position = Vector3.Lerp(startPoses[iPanel], endPoses[iPanel], Mathf.SmoothStep(0, 1, i));
                yield return null;
            }
            if(!calledNext)
                if (iPanel < targetIndex)
                    StartCoroutine(MoveRight(iPanel + 1));
        }
        
    }
    IEnumerator MoveLeft(int iPanel)
    {
        if (iPanel <= targetIndex)
        {
            yield return null;
        }
        else
        {
            float i = inverseLerpLeft(iPanel);
            bool calledNext = false;
            while (i < 1)
            {
                if (!calledNext && i > 0.15f)
                {
                    calledNext = true;
                    if (iPanel > targetIndex)
                        StartCoroutine(MoveLeft(iPanel - 1));
                }
                i += Time.deltaTime * 5;
                panels[iPanel].rectTransform.position = Vector3.Lerp(endPoses[iPanel], startPoses[iPanel], Mathf.SmoothStep(0, 1, i));
                yield return null;
            }
            if (!calledNext)
            {
                if (iPanel > targetIndex)
                    StartCoroutine(MoveLeft(iPanel - 1));
            }
        }
       
    }

    IEnumerator Deploy(int num, float wait)
    {
        if (num > 14)
            num = 14;
        numberOfDeployedItem = num;
        yield return new WaitForSeconds(wait);
        float[] indexTab = new float[num];
        int nbItem = 1;
        while (indexTab[indexTab.Length-1] < 1)
        {
            for (var i = 0; i < nbItem; ++i)
            {
                indexTab[i] += Time.deltaTime * 4;
                panels[i].rectTransform.position = Vector3.Lerp(startPoses[i], endPoses[i], Mathf.SmoothStep(0, 1, indexTab[i]));
            }
            if (indexTab[nbItem - 1] > 0.15f && nbItem < num)
                nbItem += 1;
            yield return null;
        }
        for (var i = 0; i < nbItem; ++i)
            panels[i].rectTransform.position = Vector3.Lerp(startPoses[i], endPoses[i], 1);
        deployed = true;
    }

   
}
