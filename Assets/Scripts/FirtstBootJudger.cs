using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirtstBootJudger : MonoBehaviour
{

    string nextSceneName;

    public GameObject canvas;

    public GameObject reset_text;

    bool resitered;

    public GameObject resetPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("BootCountAfterUpdt2") < 1)
        {
            resetPanel.SetActive(true);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("BootCountAfterUpdt2", 1);
            PlayerPrefs.SetInt("BootCount", 0);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("BootCount") < 1)
        {
            resitered = false;
        }
        else
        {
            resitered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("boot:" + PlayerPrefs.GetInt("BootCount") + "registered:"+resitered);
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.P))
        {
            DeletePrefs();
            Debug.Log("Deleted");
            reset_text.SetActive(true);
        }
    }

    public void ToHalloween()
    {
        if (resitered)
        {
            nextSceneName = "Game_Halloween";
        }
        else
        {
            nextSceneName = "UserRegister_Hal";
        }
        Invoke("ToNext", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }

    public void ToXmas()
    {
        if (resitered)
        {
            nextSceneName = "Game_Xmas";
        }
        else
        {
            nextSceneName = "UserRegister_Xmas";
        }
        Invoke("ToNext", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }

    public void ToValentine()
    {
        if (resitered)
        {
            nextSceneName = "Game_Valentine";
        }
        else
        {
            nextSceneName = "UserRegister_Valentine";
        }
        Invoke("ToNext", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }

    public void NextScene()
    {
        if (resitered)
        {
            nextSceneName = "Game_Xmas";
        }
        else
        {
            nextSceneName = "UserRegister_Xmas";
        }
        Invoke("ToNext", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }


    void ToNext()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
