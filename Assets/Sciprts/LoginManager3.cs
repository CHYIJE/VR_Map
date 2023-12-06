using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager3 : MonoBehaviour
{
    private string apiBaseUrl = "http://kitcomputer.kr:5200/unity/login";

    public InputField usernameInput;
    public InputField passwordInput;
    public Text loginResultText;


    // private string idInput = "qwer1234"; // idInput을 멤버 변수로 선언

    public void LoginBtn()
    {
        StartCoroutine(Login(usernameInput.text, passwordInput.text));
    }

    

    IEnumerator Login(string idInput, string pwInput)
    {
        

        LoginData loginData = new LoginData { userId = idInput, userPassword = pwInput };
        string jsonData = JsonUtility.ToJson(loginData);

        UnityWebRequest loginRequest = UnityWebRequest.PostWwwForm(apiBaseUrl, "POST");
        loginRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        loginRequest.downloadHandler = new DownloadHandlerBuffer();
        loginRequest.SetRequestHeader("Content-Type", "application/json");

        yield return loginRequest.SendWebRequest();

        if (loginRequest.result == UnityWebRequest.Result.Success)
        {
            string responseText = loginRequest.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);

            // 세션 토큰과 클라이언트에서 사용하는 userId를 PlayerPrefs에 저장
            PlayerPrefs.SetString("AuthToken", response.token);
            PlayerPrefs.SetString("LoggedInUserId", idInput); // 클라이언트에서 사용하는 userId를 저장
            PlayerPrefs.Save();

            Debug.Log("Login successful!");
            string savedSessionToken = PlayerPrefs.GetString("AuthToken", "No session token found");
            string loggedInUserId = PlayerPrefs.GetString("LoggedInUserId", "No user ID found");

            Debug.Log("Saved Session Token: " + savedSessionToken);
            Debug.Log("Logged In UiMapUser ID: " + loggedInUserId);

            // 로그인이 성공하면 Fac1Map 씬으로 이동
            SceneManager.LoadScene("Fac1Map");
        }
        else
        {
            Debug.Log("Login failed: " + loginRequest.error);
            loginResultText.text = "Login failed: " + loginRequest.error;
        }
    }

    [Serializable]
    private class LoginData
    {
        public string userId;
        public string userPassword;
    }

    private class LoginResponse
    {
        public string isLogin;
        public string token;
    }
}
