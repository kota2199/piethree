using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToDestroy()
    {
        Invoke("StopParticle", 0.2f);
    }

    void StopParticle()
    {
        this.GetComponent<ParticleSystem>().Stop();
        Invoke("Destrarticle", 1f);
    }

    void Destrarticle()
    {
        Destroy(this.gameObject);
    }
}
