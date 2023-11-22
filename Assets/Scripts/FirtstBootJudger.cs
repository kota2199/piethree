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

    public bool registered;

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
            registered = false;
        }
        else
        {
            registered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.P))
        {
            DeletePrefs();
            Debug.Log("Deleted");
            reset_text.SetActive(true);
        }
    }
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
