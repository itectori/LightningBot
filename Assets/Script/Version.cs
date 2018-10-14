using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
	private Text version;
	
	void Start ()
	{
		print("start");
		version = GetComponent<Text>();
		StartCoroutine(Tools.WebRequest("lightningbot.tk/version", DisplayVersion));
	}

	private void DisplayVersion(string s)
	{
		version.text = "< " + s + " >";
	}
}
