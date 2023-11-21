using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public int Score;

    public Text scoreTxt,timeTxt;

    public GameObject scoreForEf, cDownTxt;

    public GameObject scoreBoard, ctdownPanel, clock, pinchiPanel;

    float curntTime, maxTime;

    bool gameEnded,gamePinchi = false;

    public GameObject saver;

    public bool isPlaying = false;

    [SerializeField] AudioSource audioSource_main, audioSource_pinchi, audioSource_result;

    public AudioClip bgm1, bgm2, bgm3;

    public bool isHolding = false;

    public GameObject canvas;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    int itemCount = 0;

    public Text itemText, clickText;

    public GameObject PieceGenerator;

    public GameObject XmasItemBtn;

    Vector3 ItemBtnSc;

    int boost = 1;

    public GameObject boostTx, honmeiEffect;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        audioSource_main = GetComponent<AudioSource>();
        audioSource_main.PlayOneShot(bgm1);
        maxTime = 60;
        curntTime = maxTime;
        StartCoroutine("CountDown");
        if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
        {
            ItemBtnSc = XmasItemBtn.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text = Score.ToString() + "pts";
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
                if(ver.ToString() == "Valentine")
                {
                    saver.GetComponent<DataSave>().saveValentineScore(Score);
                }
                else
                {
                    saver.GetComponent<DataSave>().Save(Score);
                }
                saver.GetComponent<DataSave>().ShowRanking();
                gameEnded = true;
            }
        }
        timeTxt.text = curntTime.ToString("f1") + "s";
    }

    public void GetScore()
    {
        Score += 1 * boost;
        StartCoroutine("GetScoreTextEf");
    }
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

    public void UseXmasItem()
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
        else
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
