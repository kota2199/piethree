using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceChanger : MonoBehaviour
{

    Vector3 selfPos;

    public bool catched = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Piece")
        {
            if (catched)
            {
                collision.gameObject.transform.position = selfPos;
            }
        }
    }

    public void GetPos(Vector3 pos)
    {
        selfPos = pos;
    }

    public void SendTag()
    {
        
    }
}
