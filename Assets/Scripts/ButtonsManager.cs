using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    private Manager manager;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject info;
    [SerializeField] Text text;
    void Awake()
    {
        manager = GetComponent<Manager>();
    }
    public void Exit()
    {
        GameManager.Instance.SaveInfo(manager.ActiveState(), manager.CurrentPosition());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    public void ResetWindow()
    {
        manager.OnGameLaunch();
        GameManager.Instance.SaveInfo(manager.ActiveState(), manager.CurrentPosition());
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        if (!settings.activeSelf) settings.SetActive(true);
        else settings.SetActive(false);
    }
    public void DisplayInfo()
    {
        info.SetActive(true);
    }
    public void CloseInfo()
    {
        info.SetActive(false);
    }
    public void OpenURL()
    {
        Application.OpenURL("https://www.linkedin.com/in/nazar-mazur-389ab5213/");
        text.color = Color.blue;

    }
}
