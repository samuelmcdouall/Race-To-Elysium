using UnityEngine;
using System.Collections.Generic;

public class CGDGroundCheck : MonoBehaviour
{
    public bool IsGrounded;
    List<string> _ignoredTags = new List<string>{"Cloud", "CharacterSelect", "AreaDenialHazard", "UltPickup", "ArachneWeb"};

    void OnTriggerStay(Collider collider)
    {
        if (collider != null && !_ignoredTags.Contains(collider.gameObject.tag))
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
