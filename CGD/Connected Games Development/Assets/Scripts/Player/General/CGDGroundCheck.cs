using UnityEngine;

public class CGDGroundCheck : MonoBehaviour
{
    public bool IsGrounded;

    void OnTriggerStay(Collider collider)
    {
        if (collider != null && collider.gameObject.tag != "Cloud")
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        IsGrounded = false;
    }
}
