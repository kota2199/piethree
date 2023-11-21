using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class UserRegister : MonoBehaviour
{

    [SerializeField] InputField nameField,teamField;

    public string userName, teamName;

    string myid;

    public GameObject canvas;

    public Text team, user;

    public GameObject caution;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register()
    {
        userName = nameField.text;
        //teamName = teamField.text;
        Debug.Log(userName);
        if (userName == "" && teamName == "")
        {
            caution.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("UserName", userName);
            PlayerPrefs.SetString("Team", teamName);
            PlayerPrefs.Save();

            //NCMBObject obj = new NCMBObject("HighScore");
            if (ver.ToString() == "Halloween")
            {
                NCMBObject obj = new NCMBObject("HighScore");
                obj["Name"] = nameField.text;
                //obj["Team"] = teamField.text;
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
                        PlayerPrefs.SetString("id", selfID);
                        PlayerPrefs.Save();
                    }
                });
                nextSceneName = "Tutorial_hal";
            }
            else if (ver.ToString() == "Xmas")
            {
                NCMBObject obj = new NCMBObject("Xmas");
                obj["Name"] = nameField.text;
                //obj["Team"] = teamField.text;
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
                        PlayerPrefs.SetString("id", selfID);
                        PlayerPrefs.Save();
                    }
                });
                nextSceneName = "Tutorial_Xmas";
            }
            if (ver.ToString() == "Valentine")
            {
                NCMBObject obj = new NCMBObject("Valentine");
                obj["Name"] = nameField.text;
                //obj["Team"] = teamField.text;
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
                        PlayerPrefs.SetString("id", selfID);
                        PlayerPrefs.Save();
                    }
                });

                nextSceneName = "Tutorial_Valentine";
            }

            Invoke("ToNextScene", 1.2f);
            canvas.GetComponent<FadeInOut>().isFadeOut = true;
        }
        
    }
    void ToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("BootCount", 1);
        PlayerPrefs.Save();
    }
}
