﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemsAnimator : MonoBehaviour {
    RawImage[] panels;
    TMPro.TextMeshProUGUI[] childrenText;
    Vector3[] startPoses;
    Vector3[] endPoses;
    int targetIndex = 0;
    char rankedMode = '_';
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
        targetIndex = num - 1 < 14 ? num - 1 : 14;
        StartCoroutine(MoveRight(0));
    }
    public void FilterItem(string[] gameTab)
    {
        
        SetChildrenText(gameTab);

        int newTargetIndex = gameTab.Length-1;

        if (newTargetIndex > targetIndex)
        {
            StopAllCoroutines();
            targetIndex = newTargetIndex;
            StartCoroutine(MoveRight(0));

        }
        else if (newTargetIndex < targetIndex)
        {
            StopAllCoroutines();
            targetIndex = newTargetIndex;
            StartCoroutine(MoveLeft(14));
        }
     }
    public void HideAllItems()
    {
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
                if (gameTab[i][0] == rankedMode)
                {
                    newName = gameTab[i].Remove(0, 1);
                    panels[i].color = Color.white;
                }
                else
                    panels[i].color = Color.green;

                childrenText[i].text = newName;
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

   
}
