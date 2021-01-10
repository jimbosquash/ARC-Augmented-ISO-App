using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBounce : MonoBehaviour
{
    private Vector3 _startPosition;
    [SerializeField] private float distanceMultiplier;
    [SerializeField] private float speed;

    void Start()
    {
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = _startPosition + new Vector3(0.0f, Mathf.Sin(speed * Time.time) * distanceMultiplier, 0.0f);
    }
}
