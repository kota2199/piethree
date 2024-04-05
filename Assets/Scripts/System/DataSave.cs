using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class DataSave : MonoBehaviour
{

    public int score;

    [SerializeField]
    private Text rank_txt,scoreTxt;

    [SerializeField]
    private GameObject ranking_board;

    private bool isDisplayRanking = false;

    private float loadRanking = 59;

    private bool getRanking = false;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    private string gameMode;

    private int scoreForCompare = 0;

    void Update()
    {
        if (getRanking)
        {
            loadRanking += Time.deltaTime;
            if (loadRanking > 60)
            {
                ShowRanking();
                loadRanking = 0;
            }
        }
    }

    public void Save(int s)
    {
        switch (ver)
        {
            case Version.Valentine:
                scoreForCompare = PlayerPrefs.GetInt("V_Score");
                break;
            case Version.Halloween:
                scoreForCompare = PlayerPrefs.GetInt("H_Score");
                break;
            case Version.Xmas:
                scoreForCompare = PlayerPrefs.GetInt("X_Score");
                break;

        }
        if (scoreForCompare < s)
        {
            if (ver == Version.Halloween)
            {
                Debug.Log("Halloween");
                string userid = PlayerPrefs.GetString("id_hal");
                PlayerPrefs.SetInt("H_Score", s);
                PlayerPrefs.Save();

                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Halloween");

                query.WhereEqualTo("objectId", userid);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
                {

                    if (e == null)
                    {
                        if (objList.Count == 0)
                        {
                            //存在しない
                            Debug.Log("Obj is not found");
                        }
                        else
                        {
                            objList[0]["Score"] = s;
                            objList[0].SaveAsync();
                            ShowRanking();
                        }

                    }

                });
            }

            if (ver == Version.Xmas)
            {
                string userid = PlayerPrefs.GetString("id_xmas");
                PlayerPrefs.SetInt("X_Score", s);
                PlayerPrefs.Save();
                Debug.Log("XMAS");
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Xmas");

                query.WhereEqualTo("objectId", userid);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
                {

                    if (e == null)
                    {
                        if (objList.Count == 0)
                        {
                            //存在しない
                            Debug.Log("Obj is not found");
                        }
                        else
                        {
                            objList[0]["Score"] = s;
                            objList[0].SaveAsync();
                            ShowRanking();
                        }

                    }

                });
            }

            if (ver == Version.Valentine)
            {
                string userid = PlayerPrefs.GetString("id_val");
                Debug.Log("Valentine");
                PlayerPrefs.SetInt("V_Score", s);
                PlayerPrefs.Save();
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Valentine");

                query.WhereEqualTo("objectId", userid);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
                {

                    if (e == null)
                    {
                        if (objList.Count == 0)
                        {
                            //存在しない
                            Debug.Log("Obj is not found");
                        }
                        else
                        {
                            objList[0]["Score"] = s;
                            objList[0].SaveAsync();
                            ShowRanking();
                        }

                    }

                });
            }
            getRanking = true;
        }
        ShowRanking();
        scoreTxt.text = "Score\n" + s.ToString() + "個";
    }

    public void ShowRanking()
    {
        int count = 0;
        string tempScore = "";
        switch (ver)
        {
            case Version.Valentine:
                gameMode = "Valentine";
                break;
            case Version.Halloween:
                gameMode = "Halloween";
                break;
            case Version.Xmas:
                gameMode = "Xmas";
                break;

        }
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(gameMode);
        query.OrderByDescending("Score");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                Debug.Log("ランキング取得失敗");
            }
            else
            {
                foreach (NCMBObject obj in objList)
                {
                    count++;
                    tempScore += count.ToString() + "位：" + obj["Name"] + "：Score：" + obj["Score"] + "個" + "\r\n";

                    rank_txt.text = tempScore.ToString();
                }
                Debug.Log("ランキング取得成功");
            }
        });
    }

    public void DisplayRanking()
    {
        if (isDisplayRanking == false)
        {
            ranking_board.SetActive(true);
            isDisplayRanking = true;
        }
        else
        {
            ranking_board.SetActive(false);
            isDisplayRanking = false;
        }
    }

}
