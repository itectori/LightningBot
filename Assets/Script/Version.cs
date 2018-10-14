using UnityEngine;
using UnityEngine.UI;

namespace Script
{
	public class Version : MonoBehaviour
	{
		private Text version;

		private void Start ()
		{
			version = GetComponent<Text>();
			StartCoroutine(Tools.WebRequest("lightningbot.tk/version", DisplayVersion));
		}

		private void DisplayVersion(string s)
		{
			version.text = "< " + s + " >";
		}
	}
}
