using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理
    float red, green, blue, alfa;   //パネルの色、不透明度を管理

    public bool isFadeOut = false;
    public bool isFadeIn = true;

    public GameObject FadePanel;

    // Start is called before the first frame update
    void Start()
    {
        red = FadePanel.GetComponent<Image>().color.r;
        green = FadePanel.GetComponent<Image>().color.g;
        blue = FadePanel.GetComponent<Image>().color.b;
        alfa = FadePanel.GetComponent<Image>().color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            StartFadeIn();
        }

        if (isFadeOut)
        {
            StartFadeOut();
        }
    }
    void StartFadeIn()
    {
        alfa -= fadeSpeed;                //a)不透明度を徐々に下げる
        SetAlpha();                      //b)変更した不透明度パネルに反映する
        if (alfa <= 0)
        {                    //c)完全に透明になったら処理を抜ける
            isFadeIn = false;
            FadePanel.GetComponent<Image>().enabled = false;    //d)パネルの表示をオフにする
            FadePanel.SetActive(false);
        }
    }
    void StartFadeOut()
    {
        FadePanel.SetActive(true);
        FadePanel.GetComponent<Image>().enabled = true;
        alfa += fadeSpeed;         // b)不透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (alfa >= 1)
        {             // d)完全に不透明になったら処理を抜ける
            isFadeOut = false;
        }
    }

    void SetAlpha()
    {
        FadePanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
    }
}
