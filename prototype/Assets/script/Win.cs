using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void Button_MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Button_Battle()
    {
        SceneManager.LoadScene("Battle");
    }
}
