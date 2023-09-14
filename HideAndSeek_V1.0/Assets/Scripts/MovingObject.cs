using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private PathPoints_01 _pathpoint;

    [SerializeField]
    private float _speed;

    private int _targetPathPointIndex;

    private Transform _previousPathpoint;
    private Transform _targetPathpoint;

    private float _timeToPathpoint;
    private float _elapsedTime;
    private bool shouldMove = false;

    void Start()
    {
        StartCoroutine(StartMovingAfterDelay(20f));
    }

    IEnumerator StartMovingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shouldMove = true;
        TargetNextPathpoint();
    }

    void FixedUpdate()
    {
        if(!shouldMove) return;
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToPathpoint;

        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(
            _previousPathpoint.position,
            _targetPathpoint.position,
            elapsedPercentage
        );
        transform.rotation = Quaternion.Lerp(
            _previousPathpoint.rotation,
            _targetPathpoint.rotation,
            elapsedPercentage
        );

        if (elapsedPercentage >= 1)
        {
            TargetNextPathpoint();
        }
    }

    private void TargetNextPathpoint()
    {
        _previousPathpoint = _pathpoint.GetPathpoint(_targetPathPointIndex);
        _targetPathPointIndex = _pathpoint.GetNextPathpointIndex(_targetPathPointIndex);
        _targetPathpoint = _pathpoint.GetPathpoint(_targetPathPointIndex);

        _elapsedTime = 0;

        float distanceToPathpoint = Vector3.Distance(
            _previousPathpoint.position,
            _targetPathpoint.position
        );
        _timeToPathpoint = distanceToPathpoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
