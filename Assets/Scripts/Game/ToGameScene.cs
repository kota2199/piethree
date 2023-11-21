using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameScene : MonoBehaviour
{

    public GameObject canvas;

    public AudioClip se1;

    AudioSource audioSource;

    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToGame()
    {
        audioSource.PlayOneShot(se1);
        Invoke("NextScene", 1.2f);
        canvas.GetComponent<FadeInOut>().isFadeOut = true;
    }

    void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
