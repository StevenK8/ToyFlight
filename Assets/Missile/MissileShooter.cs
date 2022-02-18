using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR;

public class MissileShooter : MonoBehaviour
{
    public AudioSource sound;
    public int impulseFactor = 100;
    public float ballLifeTime = 10f;

    public GameObject missile;

    public GameObject avion;
    private List<float> creationStamp;
    private List<GameObject> ballsGameObjects;
    private bool lastTriggerValue = false;
    private float timer;
    private bool canShoot = true;

    public float inbetweenShootsTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        creationStamp = new List<float>();
        ballsGameObjects = new List<GameObject>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < creationStamp.Count; i++){
            if(Time.time - creationStamp[i] > ballLifeTime){
                Destroy(ballsGameObjects[i]);
            }
        }
        timer += Time.deltaTime;
        canShoot = timer > inbetweenShootsTime;
    }

    public void Shoot(){
        if(canShoot){
            Rigidbody ball  = CreateBall();
            Launch(ball);
            sound.Play();
            timer = 0;
        }
        
    }

    private void Launch(Rigidbody rb){
        Vector3 f = transform.forward;
        f.x *= impulseFactor;
        f.y *= impulseFactor;
        f.z *= impulseFactor;
        rb.AddForce(f, ForceMode.Impulse);
    }

    public Rigidbody CreateBall(){
        GameObject m = Instantiate(missile, avion.transform.position+(new Vector3(0f, -1f, 3f)),transform.rotation);


        //m.transform.position = transform.position;
        m.transform.localScale = new Vector3(0.03f,0.03f,0.03f);
        ballsGameObjects.Add(m);

        Rigidbody rb = m.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        creationStamp.Add(Time.time);

        return m.GetComponent<Rigidbody>();
    }

    public void OnTriggeredPressed(bool triggerValue){
        if(isActiveAndEnabled && !lastTriggerValue && triggerValue){
            Shoot();
        }
        lastTriggerValue = triggerValue;
    }
    
}
