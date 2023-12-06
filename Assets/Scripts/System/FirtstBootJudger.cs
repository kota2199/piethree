using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirtstBootJudger : MonoBehaviour
{
    public GameObject canvas;

    public GameObject reset_text;

    public bool registered;

    public GameObject resetPanel;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.P))
        {
            DeletePrefs();
            Debug.Log("Deleted");
            reset_text.SetActive(true);
        }
    }

    public void HalRegistered()
    {
        PlayerPrefs.SetInt("HalRegistered", 1);
        PlayerPrefs.Save();
    }

    public void ValRegistered()
    {
        PlayerPrefs.SetInt("ValRegistered", 1);
        PlayerPrefs.Save();
    }

    public void XmasRegistered()
    {
        PlayerPrefs.SetInt("XmasRegistered", 1);
        PlayerPrefs.Save();
    }
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
