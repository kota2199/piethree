using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{

    public enum SE
    {
        se1, se2, se3
    }

    public AudioClip clip1, clip2,clip3;

    AudioSource audioSource;

    // 列挙型の列挙値を格納する変数
    public SE seName;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE()
    {
        if(seName == SE.se1)
        {
            audioSource.PlayOneShot(clip1);
        }
        if (seName == SE.se2)
        {
            audioSource.PlayOneShot(clip2);
        }
        if (seName == SE.se3)
        {
            audioSource.PlayOneShot(clip3);
        }
    }
}
