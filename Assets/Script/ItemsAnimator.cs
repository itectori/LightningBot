using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemsAnimator : MonoBehaviour {
    RawImage[] children;
    TMPro.TextMeshProUGUI[] childrenText;
    Vector3[] startPoses;
    Vector3[] endPoses;
    bool deployed = false;
    int numberOfDeployedItem = 0;
    [HideInInspector]
    public string[] currentGameTab = null;
    private IEnumerator deployCor;
    private IEnumerator UndeployCor;

    // Use this for initialization
    void Start () {
        children = transform.GetComponentsInChildren<RawImage>();
        childrenText = transform.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        startPoses = new Vector3[children.Length];
        endPoses = new Vector3[children.Length];
        for (int i = 0; i < children.Length; ++i)
        {
            startPoses[i] = children[i].rectTransform.position;
            endPoses[i] = startPoses[i] + new Vector3(182, 0, 0);
        }
        
	}
	

    public void ShowItem(int num, float delay=0)
    {
        StartCoroutine(Deploy(num, 0));
    }
    public void FilterItem(string[] gameTab)
    {
        int gamesToAdd = gameTab.Length - numberOfDeployedItem;
        SetChildrenText(gameTab);
        if (gamesToAdd == 0)
            return;
        print("GAMES TO ADD" + gamesToAdd);
        if (gamesToAdd < 0)
        {
            
            UndeployCor = UndeployRange(numberOfDeployedItem, gameTab.Length);
            StartCoroutine(UndeployCor);
        }
        else
        {
            
            deployCor = DeployRange(numberOfDeployedItem, gameTab.Length);
            StartCoroutine(deployCor);
        }
    }
    public void HideAllItems()
    {
        StartCoroutine(Undeploy());
    }

    public void SetChildrenText(string[] gameTab)
    {
        for (int i = 0; i < 15; ++i)
        {
            if (i >= gameTab.Length)
                childrenText[i].text = "";
            else
                childrenText[i].text = gameTab[i];
        }
        currentGameTab = gameTab;
    }
    private void checkSamePos(ref float[] indextab, RawImage[] children, int from, int to)
    {
        for (int i =from; i < to;++i)
        {
            if (children[i].rectTransform.position.x == startPoses[i].x)
                continue;
            else
            {
                indextab[i] = Mathf.InverseLerp(startPoses[i].x, endPoses[i].x, children[i].rectTransform.position.x);
                print("INDEX: " + i + "CHANGED TO: " + indextab[i]);
            }
        }
    }
    private void checkSameUnPos(ref float[] indextab, RawImage[] children, int from, int to)
    {
        for (int i = from; i < to; ++i)
        {
            if (children[i].rectTransform.position.x == endPoses[i].x)
                continue;
            else
            {
                indextab[i] = Mathf.InverseLerp(endPoses[i].x,startPoses[i].x, children[i].rectTransform.position.x);
                print("INDEX: " + i + "CHANGED TO: " + indextab[i]);
            }
        }
    }
    IEnumerator Deploy(int num, float wait)
    {
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
        for (var i = 0; i < nbItem; ++i)
            children[i].rectTransform.position = Vector3.Lerp(startPoses[i], endPoses[i], 1);
        deployed = true;
    }

    bool evertythingAtOne(int from, float[]tab)
    {
        for(;from < tab.Length;++from )
            if (tab[from] < 1)
            {
                print(from);
                return false;
            }
        return true;
    }
    IEnumerator DeployRange(int from, int to)
    {

        numberOfDeployedItem = to;
        float[] indexTab = new float[to];
        int nbItem = from+1;
        checkSamePos(ref indexTab, children, from, to);

        while (!evertythingAtOne(from,indexTab))
        {
            for (var i = from ; i < nbItem ; ++i)
            {
                indexTab[i] += Time.deltaTime * 1;
                children[i].rectTransform.position = Vector3.Lerp(startPoses[i], endPoses[i], Mathf.SmoothStep(0, 1, indexTab[i]));
            }
            if (indexTab[nbItem -1] > 0.05f && nbItem <to )
                nbItem += 1;
            yield return null;
        }
        print("FINISHED DEPLOY");
    }
        
    IEnumerator Undeploy(float wait = 0)
    {

        yield return new WaitForSeconds(wait);
        float[] indexTab = new float[numberOfDeployedItem];
        numberOfDeployedItem = 0;
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
    IEnumerator UndeployRange(int from, int to)
    {
        float[] indexTab = new float[numberOfDeployedItem];
        int nbItem = to + 1;
        numberOfDeployedItem = to;
        checkSameUnPos(ref indexTab, children, to, from);
        while (!evertythingAtOne(to, indexTab))
        {
            for (var i = to; i < nbItem ; ++i)
            {
                indexTab[i] += Time.deltaTime * 1;
                if (children[i].rectTransform.position == startPoses[i])
                    continue;
                children[i].rectTransform.position = Vector3.Lerp(endPoses[i], startPoses[i], Mathf.SmoothStep(0, 1, indexTab[i]));
            }
            if (indexTab[nbItem -1] > 0.05f && nbItem < from)
                nbItem += 1;
            yield return null;
        }
        for (var i = to; i < nbItem; ++i)
            children[i].rectTransform.position = Vector3.Lerp(endPoses[i], startPoses[i], 1);
        deployed = false;
        print("FINISHED");
    }
}
