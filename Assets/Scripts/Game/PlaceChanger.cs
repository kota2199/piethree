using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceChanger : MonoBehaviour
{

    Vector3 selfPos;

    public bool catched = false;

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
}
