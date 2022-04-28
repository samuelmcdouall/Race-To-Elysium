using System.Collections;
using UnityEngine;

public class CGDSweepingHazard : MonoBehaviour
{
    [SerializeField]
    float _speed;
    [SerializeField]
    float _fadeInOutTime;
    [SerializeField]
    float _attackInterval;
    public Transform StartPosition;
    public Transform EndPosition;
    [SerializeField]
    float _positionThreshold;
    Color _color;
    HazardState _hazardState;
    Rigidbody _rigidbody;
    Collider _collider;
    void Awake()
    {
        _hazardState = HazardState.WaitingAtStart;
        _color = GetComponent<Renderer>().material.color;
        _collider = GetComponent<Collider>();
        GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, 0.0f);
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        StartCoroutine(WaitForNextAttack(_attackInterval));
    }

    void FixedUpdate()
    {
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

        Vector3 startToEndDirection = (EndPosition.position - StartPosition.position).normalized;
        if (_hazardState == HazardState.WaitingAtStart)
        {
            _hazardState = HazardState.MovingToEnd;
            _rigidbody.velocity = startToEndDirection * _speed;
        }
        else
        {
            _hazardState = HazardState.MovingToStart;
            _rigidbody.velocity = -startToEndDirection * _speed;
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
        yield return WaitForNextAttack(_attackInterval);
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
        WaitingAtEnd
    }
}
