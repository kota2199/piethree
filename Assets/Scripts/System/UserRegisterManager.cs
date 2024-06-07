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

    //�o�^�{�^�����������Ƃ�
    public void Register()
    {
        userName = nameField.text;
        sendButtonText.text = "�o�^��...";

        //���[�U�[�l�[����������or�񕶎��ȉ��̏ꍇ(Playfab�̎d�l��2�����ȉ��͕s��)�͌x���A3�����ȏ���͂���Ă�����o�^
        if (userName == "" || userName.Length < 2)
        {
            caution.gameObject.SetActive(true);
            caution.text = "���O��3�����ȏ�œ��͂��Ă��������B";
            sendButtonText.text = "OK";
        }
        else
        {
            UpdateUserName(userName);
        }

    }

    //���͂��ꂽ���O�����Ɏg���Ă�����A�����ȕ�����ł͂Ȃ������`�F�b�N
    //PlayfabRankingManager.UpdateUserNameAsync
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

    //���[�U�[�l�[���ƁA�o�^�ς݂��ǂ��������[�J���ɕۑ�����
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
    }
}
