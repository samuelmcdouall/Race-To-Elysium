using UnityEngine;

public class CGDGateTrigger : MonoBehaviour
{
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
