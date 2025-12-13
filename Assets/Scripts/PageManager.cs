using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne deðiþtirmek için bu kütüphane þart

public class PageManager : MonoBehaviour
{
    // Butona basýnca istenilen sahneye götürür
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Uygulamadan çýkýþ yapar
    public void QuitApp()
    {
        Debug.Log("Uygulamadan çýkýldý!"); // Unity Editörde çalýþtýðýný anlamak için
        Application.Quit();
    }
}