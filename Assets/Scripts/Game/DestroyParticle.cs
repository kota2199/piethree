using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public void ToDestroy()
    {
        Invoke("StopParticle", 0.2f);
    }

    private void StopParticle()
    {
        this.GetComponent<ParticleSystem>().Stop();
        Invoke("Destrarticle", 1f);
    }

    private void Destrarticle()
    {
        Destroy(this.gameObject);
    }
}
