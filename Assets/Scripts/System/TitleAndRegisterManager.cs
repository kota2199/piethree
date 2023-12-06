using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleAndRegisterManager : MonoBehaviour
{
    public enum Mode
    {
        Valentine, Halloween, Xmas
    }

    [SerializeField]
    GameObject fadeCanvas, titleCanvas, valRegiCanvas, halRegiCanvas, xmasRegiCanvas;

    public Mode mode;

    public void ToValentineMode()
    {
        mode = Mode.Valentine;
        if (PlayerPrefs.GetInt("ValRegistered") > 0)
        {
            StartCoroutine(ToNextScene());
        }
        else
        {
            OpenRegisterCanvas();
        }
    }

    public void ToHaloweenMode()
    {
        mode = Mode.Halloween;
        if (PlayerPrefs.GetInt("HalRegistered") > 0)
        {
            StartCoroutine(ToNextScene());
        }
        else
        {
            OpenRegisterCanvas();
        }
    }

    public void ToXmasMode()
    {
        mode = Mode.Xmas;
        if (PlayerPrefs.GetInt("XmasRegistered") > 0)
        {
            StartCoroutine(ToNextScene());
        }
        else
        {
            OpenRegisterCanvas();
        }
    }

    public void OpenRegisterCanvas()
    {
        titleCanvas.SetActive(false);
        switch (mode)
        {
            case Mode.Valentine:
                valRegiCanvas.SetActive(true);
                break;
            case Mode.Halloween:
                halRegiCanvas.SetActive(true);
                break;
            case Mode.Xmas:
                xmasRegiCanvas.SetActive(true);
                break;
            default:
                break;
        }
    }

    private IEnumerator ToNextScene()
    {
        fadeCanvas.GetComponent<FadeInOut>().isFadeOut = true;
        yield return new WaitForSeconds(1f);
        switch (mode)
        {
            case Mode.Valentine:
                SceneManager.LoadScene("Game_Valentine");
                break;
            case Mode.Halloween:
                SceneManager.LoadScene("Game_Halloween");
                break;
            case Mode.Xmas:
                SceneManager.LoadScene("Game_Xmas");
                break;
            default:
                break;
        }
    }
}
