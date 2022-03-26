using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGateTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HealthBar;
    public GameObject Hazard;
    bool _triggered;

    void Start()
    {
        _triggered = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!_triggered && collision.gameObject.tag == "Player")
        {
            _triggered = true;
            Hazard.SetActive(true);
            HealthBar.SetActive(true);
        }
    }
}
