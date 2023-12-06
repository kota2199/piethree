using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class UserRegister : MonoBehaviour
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
        if (userName == "")
        {
            caution.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("UserName", userName);
            PlayerPrefs.Save();

            if (ver.ToString() == "Halloween")
            {
                NCMBObject obj = new NCMBObject("Halloween");
                obj["Name"] = nameField.text;
                obj["Score"] = 0;
                string selfID;
                obj.SaveAsync((NCMBException e) => {
                    if (e != null)
                    {
                        //エラー処理
                    }
                    else
                    {
                        //成功時の処理
                        selfID = obj.ObjectId;
                        PlayerPrefs.SetString("id_hal", selfID);
                        PlayerPrefs.Save();
                    }
                });
                nextSceneName = "Game_Halloween";
                PlayerPrefs.SetInt("HalRegistered", 1);
            }

            else if (ver.ToString() == "Xmas")
            {
                NCMBObject obj = new NCMBObject("Xmas");
                obj["Name"] = nameField.text;
                obj["Score"] = 0;
                string selfID;
                obj.SaveAsync((NCMBException e) => {
                    if (e != null)
                    {
                        //エラー処理
                    }
                    else
                    {
                        //成功時の処理
                        selfID = obj.ObjectId;
                        PlayerPrefs.SetString("id_xmas", selfID);
                        PlayerPrefs.Save();
                    }
                });
                nextSceneName = "Game_Xmas";
                PlayerPrefs.SetInt("XmasRegistered", 1);
            }

            else if (ver.ToString() == "Valentine")
            {
                NCMBObject obj = new NCMBObject("Valentine");
                obj["Name"] = nameField.text;
                obj["Score_Valentine"] = 0;
                string selfID;
                obj.SaveAsync((NCMBException e) => {
                    if (e != null)
                    {
                        //エラー処理
                    }
                    else
                    {
                        //成功時の処理
                        selfID = obj.ObjectId;
                        PlayerPrefs.SetString("id_val", selfID);
                        PlayerPrefs.Save();
                    }
                });
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
