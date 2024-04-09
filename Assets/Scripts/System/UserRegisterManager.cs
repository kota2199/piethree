using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserRegisterManager : MonoBehaviour
{
    [SerializeField] InputField nameField;

    public string userName;

    public GameObject fadeCanvas;

    public GameObject caution;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    string nextSceneName;

    public void Register()
    {
        userName = nameField.text;
        Debug.Log(userName);
        if (userName == "" || userName.Length < 2)
        {
            caution.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("UserName", userName);
            PlayerPrefs.Save();

            if (ver.ToString() == "Halloween")
            {
                nextSceneName = "Game_Halloween";
                PlayerPrefs.SetInt("HalRegistered", 1);
            }

            else if (ver.ToString() == "Xmas")
            {
                nextSceneName = "Game_Xmas";
                PlayerPrefs.SetInt("XmasRegistered", 1);
            }

            else if (ver.ToString() == "Valentine")
            {
                nextSceneName = "Game_Valentine";
                PlayerPrefs.SetInt("ValRegistered", 1);
            }


            Invoke("ToNextScene", 1.2f);
            fadeCanvas.GetComponent<FadeInOut>().isFadeOut = true;
        }

    }
    void ToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("BootCount", 1);
        PlayerPrefs.Save();
    }
}
