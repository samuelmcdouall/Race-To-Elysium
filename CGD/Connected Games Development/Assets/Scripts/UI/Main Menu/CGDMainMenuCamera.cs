using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDMainMenuCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float _cameraMoveSpeed;
    public Transform StartPosition;
    public Transform EndPosition;
    [SerializeField]
    float _waypointThreshold;
    bool _movingTowardsEnd;
    void Start()
    {
        _movingTowardsEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movingTowardsEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + _cameraMoveSpeed * Time.deltaTime, transform.position.z);
            if (Vector3.Distance(transform.position, EndPosition.position) < _waypointThreshold)
            {
                _movingTowardsEnd = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - _cameraMoveSpeed * Time.deltaTime, transform.position.z);
            if (Vector3.Distance(transform.position, StartPosition.position) < _waypointThreshold)
            {
                _movingTowardsEnd = true;
            }
        }
        float scaledColorPosition = transform.position.y / Vector3.Distance(StartPosition.position, EndPosition.position);
        if (scaledColorPosition > 1.0f)
        {
            scaledColorPosition = 1.0f;
        }
        else if (scaledColorPosition < 0.0f)
        {
            scaledColorPosition = 0.0f;
        }
        Color tartarusColor = new Color(1.0f, 0.0f, 0.0f);
        Color normalColour = new Color(0.45f, 0.45f, 0.45f);
        Color skyboxColor = Color.Lerp(tartarusColor, normalColour, scaledColorPosition);
        RenderSettings.skybox.SetColor("_Tint", skyboxColor);
    }
}
