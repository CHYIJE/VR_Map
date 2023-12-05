using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// 공정 설명 요청 코드임 받아오기

public class Getexplain : MonoBehaviour
{
    private string apiBaseUrl = "http://kitcomputer.kr:5200/unity/";
    public Text objexplaintext;
    public string selectprocess = "믹싱공정";
    public int quiznum = 0;
    private GameObject obj;
    
    void Start()
    {
        string loggedInUserId = PlayerPrefs.GetString("LoggedInUserId", "No user ID found");
        StartCoroutine(Getobjexplain());
    }

    IEnumerator Getobjexplain()
    {
        getProcess loginData = new getProcess { process = selectprocess };
        string jsonData = JsonUtility.ToJson(loginData);

        UnityWebRequest hs_get = UnityWebRequest.Get(apiBaseUrl + "OBJexplain");
        hs_get.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        hs_get.downloadHandler = new DownloadHandlerBuffer();
        hs_get.SetRequestHeader("Content-Type", "application/json");

        yield return hs_get.SendWebRequest();

        if (hs_get.result == UnityWebRequest.Result.Success)
        {
            string dataText = hs_get.downloadHandler.text;
            string dataText2 = dataText.Substring(1, dataText.Length - 2);
            Debug.Log("dataText2 : " + dataText2);
            
            ExplainData explainData = JsonUtility.FromJson<ExplainData>(dataText2);

            Debug.Log("Explain : " + explainData.Explain);
            objexplaintext.text = explainData.Explain;

        }
        else
        {
            Debug.Log("There was an error getting the Explaindata: " + hs_get.error);
        }
    }


    [Serializable]
    private class getProcess
    {
        public string process;
    }

    [Serializable]
    private class ExplainData
    {
        public string Explain;
        public int idx;
    }
}   
