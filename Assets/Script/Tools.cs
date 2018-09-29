using System;
using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public static class Tools
    {
        public static IEnumerator WebRequest(string url, Action<string> callback)
        {
            using (var www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                    Debug.Log(www.error);
                else
                    callback(www.downloadHandler.text);
            }
        }
    }
}