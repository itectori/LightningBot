using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Script;

public class GameSelection : MonoBehaviour {
    [SerializeField]
    RawImage selector;
    [SerializeField]
    ItemsAnimator itemAnim;
    Vector3 startpos;
    Vector3 endpos;

    string[] currentGames;

    bool moving = false;
    bool deployedSearch = false;

	// Use this for initialization
	void Start () {
        startpos = selector.rectTransform.position;
        endpos = startpos +  new Vector3(182, 0, 0);
	    OnClickSearcher();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickSearcher()
    {
        if (moving)
            return;
        if (deployedSearch)
        {
            StartCoroutine(UnDeploy());
            itemAnim.HideAllItems();
        }
        else
        {
            StartCoroutine(Deploy());
            GetGames();
        }
    }
    public void GetGames()
    {
        StartCoroutine(Script.Tools.WebRequest("https://lightningbot.tk/list", SetGamesAndDisplay));

    }
    private void SetGamesAndDisplay(string s)
    {
        var tabs = s.Split('\n');
        currentGames = tabs;
        itemAnim.SetChildrenText(tabs);
        itemAnim.ShowItem(tabs.Length);
    }
    public void FilterGames(string token)
    {
        if (!deployedSearch)
            return;
        string[] filteredTab = currentGames.Where(g => g.StartsWith(token)).ToArray();
        //string[] filteredTab = new string[] { "lol", "mdr" };
        if (filteredTab.Length == 1)
        {
            LoadGame(filteredTab[0]);
            return;
        }
        else if (filteredTab.Length == 0)
        {
            itemAnim.HideAllItems();
        }
        else
        {
            itemAnim.HideAllItems();
            itemAnim.SetChildrenText(filteredTab);
            itemAnim.ShowItem(filteredTab.Length);
        }
    }

    public void OnClickGame(int n)
    {
        LoadGame(itemAnim.currentGameTab[n]);
    }
    private void LoadGame(string name)
    {
        StartCoroutine(UnDeploy());
        itemAnim.HideAllItems();
        GameManager.NewGame(name);
    }
    IEnumerator Deploy()
    {
        
        moving = true;
        float i = 0;
        while(i < 1)
        {
            i += Time.deltaTime * 3;

            selector.rectTransform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, i));
            yield return null;
        }
        moving = false;
        deployedSearch = true;
    }
    IEnumerator UnDeploy()
    {
        moving = true;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * 3;

            selector.rectTransform.position = Vector3.Lerp(endpos, startpos, Mathf.SmoothStep(0.0f, 1.0f, i));
            yield return null;
        }
        moving = false;
        deployedSearch = false;
    }
}
