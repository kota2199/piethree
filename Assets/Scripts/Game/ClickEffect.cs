using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{

    public GameObject star;

    // Update is called once per frame
    void Update()
    {
        //マウスをクリックしたときにカーソルからエフェクトを出す
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 W_MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject star_eff = Instantiate(star, new Vector3(W_MousePoint.x, W_MousePoint.y,-1), Quaternion.identity);
            star_eff.GetComponent<DestroyParticle>().ToDestroy();
        }
    }
}
