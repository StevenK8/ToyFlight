using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{

    public GameObject particleEmitterPrefab;
    public float rescale;
    public float addX, addY, addZ;
    public float rotX, rotY, rotZ;
    private GameObject instance;

    public void Activate()
    {
        //instance = Instantiate(particleEmitterPrefab, new Vector3(3.96f, transform.position.y + 0.03f, -6.7f), transform.rotation);
        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x + rotX, rot.y + rotY, rot.z + rotZ);
        Quaternion modif = Quaternion.Euler(rot);
        instance = Instantiate(particleEmitterPrefab, new Vector3(transform.position.x + addX, transform.position.y + addY, transform.position.z + addZ), modif);
        /*instance.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        instance.transform.GetChild(1).gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);*/

        for (int i = 0; i < instance.transform.childCount; i++)
        {
            instance.transform.GetChild(i).gameObject.transform.localScale = new Vector3(rescale, rescale, rescale);
        }
        

    }

    public void Desactivate()
    {
        Destroy(instance);
    }
}
