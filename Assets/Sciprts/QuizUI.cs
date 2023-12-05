using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// 공정 설명 요청 코드임 받아오기

public class QuizUI : MonoBehaviour
{
    private string apiBaseUrl = "http://kitcomputer.kr:5200/unity/";
    public string selectprocess = "믹싱공정";
    public int quiznum = 0;
    public Text objquiztext;
    
    List<QuizData> quizList = new List<QuizData>();
    private string userAnswer = "";
    public GameObject uiPanel; // 문제 제출후 UI를 삭제하기 위해 필요
    
    void Start()
    {
        uiPanel.SetActive(true); // UI 를 보이고자 나타냄
        StartCoroutine(GetObjQuiz());
    }

    
    public void Obtn()
    {
        userAnswer = "1";
    }

    public void Xbt()
    {
        userAnswer = "0";
    }

    public void submitAnswer()
    {
        Postanswer(); // 문제 UI에 제출 버튼에 연결하고 정답을 선택한 후 문제를 제출하면 끝 이후 UI 삭제
        uiPanel.SetActive(false); // UI 삭제 코드
    }

    bool trueOrfalse()
    {
        if(userAnswer == quizList[quiznum].quizYN)
        {
            return true;
        } else {
            return false;
        }
    }

    // 
    IEnumerator GetObjQuiz()
    {

        // 코드 4개 복사하고 밑에 selectprocess 를 공정에 맞게 수정
        
        getProcess loginData = new getProcess { process = selectprocess };
        string jsonData = JsonUtility.ToJson(loginData);

        UnityWebRequest hs_get = UnityWebRequest.Get(apiBaseUrl + "OBJquiz");
        hs_get.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        hs_get.downloadHandler = new DownloadHandlerBuffer();
        hs_get.SetRequestHeader("Content-Type", "application/json");

        // 가져오기 요청
        yield return hs_get.SendWebRequest();

        // 요청이 성공했을 경우
        if (hs_get.result == UnityWebRequest.Result.Success)
        {
            string dataText = hs_get.downloadHandler.text;
            string dataText2 = dataText.Substring(1, dataText.Length - 2);
            Debug.Log("dataText: " + dataText);
            Debug.Log("dataText2: " + dataText2);
            
            quizList = JsonUtility.FromJson<QuizList>("{\"quizzes\":" + dataText + "}").quizzes;
            Debug.Log("퀴즈 내용"+quizList[quiznum].quiz);
            objquiztext.text = quizList[quiznum].quiz;

        }
        // 요청이 실패했을 경우
        else
        {
            Debug.Log("There was an error getting the Explaindata: " + hs_get.error);
        }

        
    }

    IEnumerator Postanswer()
    {
        string loggedInUserId = PlayerPrefs.GetString("LoggedInUserId", "No user ID found");
        Player answerData = new Player { userId = loggedInUserId, idx = quizList[quiznum].idx + 1, quizYN = trueOrfalse() };

        Debug.Log("answerData: "+answerData);
        string jsonData = JsonUtility.ToJson(answerData);
        Debug.Log("JsonData: "+jsonData);

        UnityWebRequest answerRequest = UnityWebRequest.PostWwwForm(apiBaseUrl + "QuizPoint", "POST");
        answerRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        answerRequest.downloadHandler = new DownloadHandlerBuffer();
        answerRequest.SetRequestHeader("Content-Type", "application/json");

        yield return answerRequest.SendWebRequest();

        if (answerRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successful");
        }
        else
        {
            Debug.Log("There was an error postting the answerData: " + answerRequest.error);
        }
    }


    [Serializable]
    public class getProcess
    {
        public string process;
    }

    [Serializable]
    public class QuizData
    {
        public int idx;
        public string quiz;
        public string quizYN;
    }
    [Serializable]
    public class QuizList
    {
        public List<QuizData> quizzes;
    }
    [Serializable]
    private class Player
    {
        public string userId; // string loggedInUserId = PlayerPrefs.GetString("LoggedInUserId", "No user ID found");
        public int idx; // quizList[randomNumber].quiz;
        public bool quizYN; // 여기에 정답을 비교해서 넣어야지...
    }
}   
