using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void Button_Start()
    {
        SceneManager.LoadScene("Battle");
    }

    public void Button_Tutorial()
    {
        SceneManager.LoadScene("tutorial");
    }
    public void Button_Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
