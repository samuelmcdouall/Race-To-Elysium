using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDRotateNameTag : MonoBehaviour
{
    Transform _mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_mainCameraTransform)
        {
            _mainCameraTransform = Camera.main.transform;
        }
        transform.LookAt(transform.position + _mainCameraTransform.rotation * Vector3.forward, _mainCameraTransform.rotation * Vector3.up);
    }
}
