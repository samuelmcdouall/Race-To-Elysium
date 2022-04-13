using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGroundCheck : MonoBehaviour
{
    public bool IsGrounded;
    void OnTriggerStay(Collider collider)
    {
        if (collider != null)
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
