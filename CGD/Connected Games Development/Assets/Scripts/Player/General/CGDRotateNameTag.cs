using UnityEngine;

public class CGDRotateNameTag : MonoBehaviour
{
    Transform _mainCameraTransform;

    void Update()
    {
        if (!_mainCameraTransform) // Continually checked for when new character spawned in
        {
            _mainCameraTransform = Camera.main.transform;
        }
        transform.LookAt(transform.position + _mainCameraTransform.rotation * Vector3.forward, _mainCameraTransform.rotation * Vector3.up);
    }
}
