using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGateTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HealthBar;
    public GameObject Hazard;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Hazard.SetActive(true);
            HealthBar.SetActive(true);
            Destroy(gameObject);
        }
    }
}
