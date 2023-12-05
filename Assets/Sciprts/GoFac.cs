using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GoFac : MonoBehaviour
{
    public GameObject FacUI;
    public void GoFac1()
    {
        SceneManager.LoadScene("Fac1Map");
    }
    public void GoFac2()
    {
        SceneManager.LoadScene("Fac2Map");
    }
    public void GoFac3()
    {
        SceneManager.LoadScene("Fac3Map");
    }
    public void GoFac4()
    {
        SceneManager.LoadScene("Fac4Map");
    }
    public void GoUIMap()
    {
        SceneManager.LoadScene("UIMap1");
    }
}
