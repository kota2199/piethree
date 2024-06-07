using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{    
    //神クラスすぎるので、夏のアップデートで切り分ける
    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    [SerializeField]
    private int Score;

    [SerializeField]
    private Text scoreTxt,timeTxt;

    [SerializeField]
    private GameObject scoreForEf, cDownTxt;

    [SerializeField]
    private GameObject scoreBoard, ctdownPanel, clock, pinchiPanel;

    [SerializeField]
    private Text selfScoreOnResult;

    private float curntTime, maxTime;

    private bool gameEnded,gamePinchi = false;

    [SerializeField]
    private GameObject saver;

    public bool isPlaying = false;

    [SerializeField]
    private AudioSource audioSource_main, audioSource_pinchi, audioSource_result;

    [SerializeField]
    private AudioClip bgm1, bgm2, bgm3;

    public bool isHolding = false;

    [SerializeField]
    private GameObject canvas;

    private int itemCount = 0;

    [SerializeField]
    private Text itemText, clickText;

    [SerializeField]
    private GameObject PieceGenerator;

    [SerializeField]
    private GameObject XmasItemBtn;

    private Vector3 ItemBtnSc;

    private int boost = 1;

    [SerializeField]
    private GameObject boostTx, honmeiEffect;

    [SerializeField]
    private GameObject rankingManager;

    [SerializeField]
    private Text rankBoard;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;

        audioSource_main = GetComponent<AudioSource>();

        audioSource_main.PlayOneShot(bgm1);

        maxTime = 60;
        curntTime = maxTime;

        GameObject.Find("PlayfabRankingManager").GetComponent<PlayfabRanking>()._rankingText = rankBoard;

        rankingManager = GameObject.Find("PlayfabRankingManager");

        SetGameMode();

        StartCoroutine("CountDown");

        if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
        {
            ItemBtnSc = XmasItemBtn.transform.localScale;
        }
    }

    //前のシーンからDontDestroyOnLoadで保持しているPlayfabRankingManagerのゲームモードを実際のシーンのモードに切り替え
    private void SetGameMode()
    {
        PlayfabRanking playfabRanking = GameObject.Find("PlayfabRankingManager").GetComponent<PlayfabRanking>();

        switch (ver)
        {
            case Version.Halloween:
                playfabRanking.gameMode = PlayfabRanking.GameMode.Haloween;
                break;

            case Version.Xmas:
                playfabRanking.gameMode = PlayfabRanking.GameMode.Xmas;
                break;

            case Version.Valentine:
                playfabRanking.gameMode = PlayfabRanking.GameMode.Valentine;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if (isPlaying)
        {
            curntTime -= Time.deltaTime;
        }
        if (curntTime <= 15)
        {
            if (!gamePinchi)
            {
                StartCoroutine("Pinchi");
                audioSource_main.Stop();
                audioSource_pinchi.PlayOneShot(bgm2);
                gamePinchi = true;
            }
        }
        if (curntTime <= 0)
        {
            curntTime = 0;
            if (!gameEnded)
            {
                isPlaying = false;
                clock.GetComponent<Clock>().isPassing = false;
                scoreBoard.SetActive(true);
                audioSource_main.Stop();
                audioSource_main.enabled = false;
                audioSource_pinchi.enabled = false;
                audioSource_result.PlayOneShot(bgm3);

                selfScoreOnResult.text = Score.ToString() + "個";

                gameEnded = true;


                rankingManager.GetComponent<PlayfabRanking>().UpdatePlayerStatistics(Score);
            }
        }
    }

    private void UpdateUI()
    {
        scoreTxt.text = Score.ToString() + "pts";
        timeTxt.text = curntTime.ToString("f1") + "s";
    }

    //スコア加点
    public void GetScore()
    {
        Score += 1 * boost;
        StartCoroutine("GetScoreTextEf");
    }
    //スコア減点
    public void LoseScore()
    {
        Score --;
        StartCoroutine("LoseScoreTextEf");
    }

    private IEnumerator GetScoreTextEf()
    {
        string colorString = "#fc821f"; // 赤色の16進数文字列
        Color newColor;
        ColorUtility.TryParseHtmlString(colorString, out newColor);
        scoreForEf.GetComponent<Text>().text = "+1";
        scoreForEf.GetComponent<Text>().color = newColor;
        scoreForEf.SetActive(true);
        yield return new WaitForSeconds(2);
        scoreForEf.SetActive(false);
    }
    private IEnumerator LoseScoreTextEf()
    {
        string colorString = "#612d7f"; // 赤色の16進数文字列
        Color newColor;
        ColorUtility.TryParseHtmlString(colorString, out newColor);
        scoreForEf.GetComponent<Text>().text = "-1";
        scoreForEf.GetComponent<Text>().color = newColor;
        scoreForEf.SetActive(true);
        yield return new WaitForSeconds(2);
        scoreForEf.SetActive(false);
    }

    public void Retery()
    {
        isPlaying = false;
        Invoke("ToNextScene", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }
    public void ToTitle()
    {
        isPlaying = false;
        Invoke("ToTitleScene", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }

    void ToNextScene()
    {
        if(ver.ToString() == "Xmas")
        {
            SceneManager.LoadScene("Game_Xmas");
        } else if (ver.ToString() == "Halloween")
        {
            SceneManager.LoadScene("Game_Halloween");
        }
        else if (ver.ToString() == "Valentine")
        {
            SceneManager.LoadScene("Game_Valentine");
        }
    }

    void ToTitleScene()
    {
        SceneManager.LoadScene("01_Title");
    }

    //ゲーム開始前のカウントダウン
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2);
        cDownTxt.GetComponent<Text>().text = 2.ToString();
        yield return new WaitForSeconds(1);
        cDownTxt.GetComponent<Text>().text = 1.ToString();
        yield return new WaitForSeconds(1);
        cDownTxt.GetComponent<Text>().text = "Go";
        clock.GetComponent<Clock>().isPassing = true;
        ctdownPanel.SetActive(false);
        isPlaying = true;
        yield return new WaitForSeconds(1);
        cDownTxt.SetActive(false);
    }

    public void Holding()
    {
        isHolding = true;
    }

    public void NoHolding()
    {
        isHolding = false;
    }

    //残り10秒で画面が点滅する
    private IEnumerator Pinchi()
    {
        for (int i = 0; i < 14; i++)
        {
            pinchiPanel.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            pinchiPanel.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    //コンボアイテムの加算
    public void ItemCountPlus()
    {
        itemCount++;
        if (ver.ToString() == "Valentine")
        {
            itemText.text = itemCount.ToString() + "/20";
            if (itemCount >= 20)
            {
                StartCoroutine("ClickText");
            }
        }
        else
        {
            itemText.text = itemCount.ToString() + "/10";
            if (itemCount >= 10)
            {
                StartCoroutine("ClickText");
            }
        }
        XmasItemBtn.transform.localScale = Vector3.Lerp(ItemBtnSc, XmasItemBtn.transform.localScale * 1.3f, 0.4f);
        Invoke("RecovertBtnSize", 0.4f);
        
    }

    public void itemCountMinus()
    {
        itemCount = 0;
        itemText.text = itemCount.ToString() + "/10";
        XmasItemBtn.transform.localScale = Vector3.Lerp(ItemBtnSc, XmasItemBtn.transform.localScale * 0.7f, 0.4f);
        Invoke("RecovertBtnSize", 0.4f);
    }

    void RecovertBtnSize()
    {
        XmasItemBtn.transform.localScale = Vector3.Lerp(XmasItemBtn.transform.localScale, ItemBtnSc, 0.4f);
        StopCoroutine("ClickText");
        clickText.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (ver.ToString() == "Valentine")
        {
            if (itemCount >= 20 && !GameObject.FindWithTag("Generator").GetComponent<PieceGenerator>().ChekingHolding())
            {
                boost = 10;
                boostTx.SetActive(true);
                honmeiEffect.SetActive(true);
                Invoke("EndBoost", 10f);
                itemCount = 0;
                itemText.text = itemCount.ToString() + "/10";
                StopCoroutine("ClickText");
                clickText.gameObject.SetActive(false);
            }
        }
        else if (ver.ToString() == "Xmas")
        {
            if (itemCount >= 10 && !GameObject.FindWithTag("Generator").GetComponent<PieceGenerator>().ChekingHolding())
            {
                PieceGenerator.GetComponent<PieceGenerator>().AllDeleteItem();
                itemCount = 0;
                itemText.text = itemCount.ToString() + "/10";
                StopCoroutine("ClickText");
                clickText.gameObject.SetActive(false);
            }
        }
    }

    //アイテムボタンがクリックできることを示すエフェクト
    private IEnumerator ClickText()
    {
        while (true)
        {
            clickText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            clickText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    void EndBoost()
    {
        boost = 1;
        boostTx.SetActive(false);
        honmeiEffect.SetActive(false);
    }
}
