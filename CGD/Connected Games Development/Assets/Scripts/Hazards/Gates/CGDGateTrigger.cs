using UnityEngine;

public class CGDGateTrigger : MonoBehaviour
{
    public GameObject HealthBar;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            HealthBar.SetActive(true);
            Destroy(gameObject);
        }
    }
}
