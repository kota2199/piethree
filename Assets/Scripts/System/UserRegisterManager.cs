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

    //登録ボタンを押したとき
    public void Register()
    {
        userName = nameField.text;
        sendButtonText.text = "登録中...";

        //ユーザーネームが未入力or二文字以下の場合(Playfabの仕様上2文字以下は不可)は警告、3文字以上入力されていたら登録
        if (userName == "" || userName.Length < 2)
        {
            caution.gameObject.SetActive(true);
            caution.text = "名前を3文字以上で入力してください。";
            sendButtonText.text = "OK";
        }
        else
        {
            UpdateUserName(userName);
        }

    }

    //入力された名前が既に使われていたり、無効な文字列ではないかをチェック
    //PlayfabRankingManager.UpdateUserNameAsync
    private async void UpdateUserName(string name)
    {
        PlayfabUserName playfabUserName = GameObject.Find("PlayfabRankingManager").GetComponent<PlayfabUserName>();
        bool isSuccess = await playfabUserName.UpdateUserNameAsync(name);

        if (isSuccess)
        {
            Debug.Log("ユーザ名の更新に成功しました。");
            RegisterSuccess();
        }
        else
        {
            Debug.Log("ユーザ名の更新に失敗しました。");
            caution.gameObject.SetActive(true);
            caution.text = "この名前は既に使われているか、無効です。";
            sendButtonText.text = "OK";
        }
    }

    //ユーザーネームと、登録済みかどうかをローカルに保存する
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
