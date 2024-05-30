using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserRegisterManager : MonoBehaviour
{
    [SerializeField] 
    private InputField nameField;

    [SerializeField]
    private string userName;

    [SerializeField]
    private GameObject fadeCanvas;

    [SerializeField]
    private Text sendButtonText;

    [SerializeField]
    private Text caution;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    private string nextSceneName;

    public void Register()
    {
        userName = nameField.text;
        Debug.Log(userName);
        sendButtonText.text = "�o�^��...";

        if (userName == "")
        {
            caution.gameObject.SetActive(true);
            caution.text = "���O����͂��Ă��������B";
            sendButtonText.text = "OK";
        }
        else
        {
            UpdateUserName(userName);
        }

    }

    private async void UpdateUserName(string name)
    {
        PlayfabUserName playfabUserName = GameObject.Find("PlayfabRankingManager").GetComponent<PlayfabUserName>();
        bool isSuccess = await playfabUserName.UpdateUserNameAsync(name);

        if (isSuccess)
        {
            Debug.Log("���[�U���̍X�V�ɐ������܂����B");
            RegisterSuccess();
        }
        else
        {
            Debug.Log("���[�U���̍X�V�Ɏ��s���܂����B");
            caution.gameObject.SetActive(true);
            caution.text = "���̖��O�͊��Ɏg���Ă��邩�A�����ł��B";
            sendButtonText.text = "OK";
        }
    }

    private void RegisterSuccess()
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

    void ToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("BootCount", 1);
        PlayerPrefs.Save();
    }
}
