using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGDGateHazardSweeping : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float _speed;
    [SerializeField]
    float _fadeInOutTime;
    [SerializeField]
    float _attackInterval;
    Color _color;
    HazardState _hazardState;
    Rigidbody _rigidbody;
    Collider _collider;
    public Transform StartPosition;
    public Transform EndPosition;
    [SerializeField]
    float _positionThreshold;
    [System.NonSerialized]
    public bool Completed;
    void Awake()
    {
        _hazardState = HazardState.WaitingAtStart;
        _color = GetComponent<Renderer>().material.color;
        _collider = GetComponent<Collider>();
        GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, 0.0f);
        _rigidbody = GetComponent<Rigidbody>();
        Completed = false;
        gameObject.SetActive(false);
        print("should see this on initial startup");
    }
    void Start()
    {
        print("shouldn't see this on initial startup");
        StartCoroutine(WaitForNextAttack(_attackInterval));
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Renderer>().material.color = new Color(1.0f,1.0f,0.4f,1.0f);
        if (_hazardState == HazardState.MovingToEnd)
        {
            if (Vector3.Distance(transform.position, EndPosition.position) <= _positionThreshold)
            {
                _collider.enabled = false;
                _rigidbody.velocity = Vector3.zero;
                StartCoroutine(FadeOut(_fadeInOutTime));
            }
        }
        else if (_hazardState == HazardState.MovingToStart)
        {
            if (Vector3.Distance(transform.position, StartPosition.position) <= _positionThreshold)
            {
                _collider.enabled = false;
                _rigidbody.velocity = Vector3.zero;
                StartCoroutine(FadeOut(_fadeInOutTime));
            }
        }
    }

    IEnumerator FadeIn(float fadeInDuration)
    {
        float fadeInTimer = 0.0f;

        while (fadeInTimer < fadeInDuration)
        {
            float fadedOpacity = Mathf.Lerp(0.0f, 1.0f, fadeInTimer / fadeInDuration);
            GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, fadedOpacity);
            fadeInTimer += Time.deltaTime;
            yield return null;
        }
        GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, 1.0f);
        if (_hazardState == HazardState.WaitingAtStart)
        {
            _hazardState = HazardState.MovingToEnd;
            _rigidbody.velocity = new Vector3(-_speed, 0.0f, 0.0f);
        }
        else
        {
            _hazardState = HazardState.MovingToStart;
            _rigidbody.velocity = new Vector3(_speed, 0.0f, 0.0f);
        }
        _collider.enabled = true;
        yield return null;
    }
    IEnumerator FadeOut(float fadeOutDuration)
    {
        float fadeOutTimer = 0.0f;

        if (_hazardState == HazardState.MovingToEnd)
        {
            _hazardState = HazardState.WaitingAtEnd;
        }
        else
        {
            _hazardState = HazardState.WaitingAtStart;
        }

        while (fadeOutTimer < fadeOutDuration)
        {
            float fadedOpacity = Mathf.Lerp(1.0f, 0.0f, fadeOutTimer / fadeOutDuration);
            GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, fadedOpacity);
            fadeOutTimer += Time.deltaTime;
            yield return null;
        }
        GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, 0.0f);
        if (Completed)
        {
            _hazardState = HazardState.Completed;
            yield return null;
        }
        else
        {
            yield return WaitForNextAttack(_attackInterval);
        }
    }

    IEnumerator WaitForNextAttack(float attackInterval)
    {
        yield return new WaitForSeconds(attackInterval);
        yield return StartCoroutine(FadeIn(_fadeInOutTime));
    }

    enum HazardState {
        MovingToEnd,
        MovingToStart,
        WaitingAtStart,
        WaitingAtEnd,
        Completed
    }


}
