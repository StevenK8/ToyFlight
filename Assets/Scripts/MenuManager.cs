using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject target;   // <--- link via inspector

    public bool showTarget;
    bool m_oldValue;

    void Start()
    {
        // ensure old value is different in order to trigger toggle at Start() if you set false in inspector
        m_oldValue = !showTarget;
    }

    void Update()
    {
        if (m_oldValue != showTarget)
        {
            m_oldValue = showTarget;
            target.SetActive(showTarget);
        }
    }

    public void Toggle()
    {
        showTarget = !showTarget;
    }

    public void NotImplYet(GameObject gb)
    {
        gb.SetActive(true);
        Rigidbody rb = target.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}