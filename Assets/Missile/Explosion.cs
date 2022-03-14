using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    void OnCollisionEnter(Collision collision)
    {
        // if collision is with AirCraft Jet object
        if (collision.gameObject.tag != "AircraftJet (2)")
        {
            // instace the prefab at our position and rotation
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            // then destroy the game object that this component is attached to
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
