using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR;
using System.Threading.Tasks;

public class MissileShooter : MonoBehaviour
{
    public AudioSource soundMissile;

    public AudioSource soundBalle;
    public int impulseFactor = 100;
    public float ballLifeTime = 10f;

    public GameObject missile;
    public GameObject balle;

    public GameObject avion;
    private List<float> creationStamp;
    private List<GameObject> ballsGameObjects;
    private bool lastTriggerValue = false;
    private float timer;
    private bool canShoot = true;

    private bool shootMissile = true;

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
        //canShoot = timer > inbetweenShootsTime;
    }

    public void Shoot(){
        if(shootMissile){
            if(canShoot){
                Rigidbody ball  = CreateBall();
                Launch(ball);
                soundMissile.Play();
                timer = 0;
            }
        }
        else{
            Rigidbody ball  = CreateBall();
            Launch(ball);
            soundBalle.Play();
        }
        
        
    }

    public void changeAmmo(){
        shootMissile = !shootMissile;
    }

    private void Launch(Rigidbody rb){
        Vector3 f = avion.transform.forward;
        f.x *= impulseFactor;
        f.y *= impulseFactor;
        f.z *= impulseFactor;

        new WaitForSeconds(0.1f);

        rb.AddForce(f, ForceMode.Impulse);

       
    }


    public Rigidbody CreateBall(){

        GameObject m;
        if(shootMissile){
            m = Instantiate(missile, avion.transform.position+(new Vector3(0f, -2f, 3.5f)),avion.transform.rotation);
        }
        else{
            m = Instantiate(balle, avion.transform.position+(new Vector3(0f, 0f, 10f)),avion.transform.rotation);
        }
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
