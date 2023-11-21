using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class DataSave : MonoBehaviour
{

    public int score;

    [SerializeField] Text rank_txt,scoreTxt, selfRankTxt;
    [SerializeField] GameObject ranking_board;

    bool isDisplayRanking = false;

    float loadRanking = 59;

    bool getRanking = false;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    //string id;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
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
        int scoreForCompare = PlayerPrefs.GetInt("Score");
        if (scoreForCompare < s)
        {
            PlayerPrefs.SetInt("Score", s);
            PlayerPrefs.Save();
            string userid = PlayerPrefs.GetString("id");
            if(ver.ToString() == "Halloween")
            {
                //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

                query.WhereEqualTo("objectId", userid);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

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
                    else
                    {

                    }

                });
            }

            if (ver.ToString() == "Xmas")
            {
                Debug.Log("XMAS");
                //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Xmas");

                query.WhereEqualTo("objectId", userid);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

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
                    else
                    {

                    }

                });
            }
            scoreTxt.text = "Score : " + s.ToString() + "個";
        }
        else
            {
                scoreTxt.text = "Score : " + s.ToString() + "個";
            }
        getRanking = true;
    }

    public void saveValentineScore(int s)
    {
        int scoreForCompare_valentine = PlayerPrefs.GetInt("Score_Valentine");
        if (scoreForCompare_valentine < s)
        {
            PlayerPrefs.SetInt("Score_Valentine", s);
            PlayerPrefs.Save();
            string userid = PlayerPrefs.GetString("id");
            if (ver.ToString() == "Valentine")
            {
                Debug.Log("VALETINE");
                //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
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
                            objList[0]["Score_Valentine"] = s;
                            objList[0].SaveAsync();
                            ShowRanking();
                        }

                    }

                });
            }
        }
        else
        {
            scoreTxt.text = "Score : " + s.ToString() + "個";
        }
        getRanking = true;
    }


    public void ShowRanking()
    {
        int count = 0;
        string tempScore = "";
        //★ データストアの「data」クラスから検索
        if (ver.ToString() == "Halloween")
        {
            //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            //★Scoreフィールドの降順でデータを取得
            query.OrderByDescending("Score");
            query.Limit = 10;
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e != null)
                {
                    UnityEngine.Debug.Log("ランキング取得失敗");
                }
                else
                {
                    //検索成功時の処理
                    UnityEngine.Debug.Log("ランキング取得成功");
                    // 値とインデックスのペアをループ処理
                    foreach (NCMBObject obj in objList)
                    {
                        count++;
                        //★ユーザーネームとスコアを画面表示
                        //tempScore += count.ToString() + "位：" + obj["Team"] + "班/" + obj["Name"] + "：Score：" + obj["Score"] +"個" + "\r\n";
                        tempScore += count.ToString() + "位：" + obj["Name"] + "：Score：" + obj["Score"] + "個" + "\r\n";
                    }
                    rank_txt.text = tempScore.ToString() + "個";
                }
            });
        }

        if (ver.ToString() == "Xmas")
        {
            //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Xmas");
            //★Scoreフィールドの降順でデータを取得
            query.OrderByDescending("Score");
            query.Limit = 10;
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e != null)
                {
                    UnityEngine.Debug.Log("ランキング取得失敗");
                }
                else
                {
                    //検索成功時の処理
                    UnityEngine.Debug.Log("ランキング取得成功");
                    // 値とインデックスのペアをループ処理
                    foreach (NCMBObject obj in objList)
                    {
                        count++;
                        //★ユーザーネームとスコアを画面表示
                        //tempScore += count.ToString() + "位：" + obj["Team"] + "班/" + obj["Name"] + "：Score：" + obj["Score"] +"個" + "\r\n";
                        tempScore += count.ToString() + "位：" + obj["Name"] + "：Score：" + obj["Score"] + "個" + "\r\n";
                    }
                    rank_txt.text = tempScore.ToString() + "個";
                }
            });
        }

        if (ver.ToString() == "Valentine")
        {
            //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Valentine");
            //★Scoreフィールドの降順でデータを取得
            query.OrderByDescending("Score_Valentine");
            query.Limit = 10;
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e != null)
                {
                    UnityEngine.Debug.Log("ランキング取得失敗");
                }
                else
                {
                    //検索成功時の処理
                    UnityEngine.Debug.Log("ランキング取得成功");
                    // 値とインデックスのペアをループ処理
                    foreach (NCMBObject obj in objList)
                    {
                        count++;
                        //★ユーザーネームとスコアを画面表示
                        //tempScore += count.ToString() + "位：" + obj["Team"] + "班/" + obj["Name"] + "：Score：" + obj["Score"] +"個" + "\r\n";
                        tempScore += count.ToString() + "位：" + obj["Name"] + "：Score：" + obj["Score_Valentine"] + "個" + "\r\n";
                        //if (obj["objectId"].ToString() == PlayerPrefs.GetString("id"))
                        //{
                        //    selfRankTxt.text = count.ToString() + "位";
                        //}

                    }
                    rank_txt.text = tempScore.ToString() + "個";
                }
            });
        }
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
