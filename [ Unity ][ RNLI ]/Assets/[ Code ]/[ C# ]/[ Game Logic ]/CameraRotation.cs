using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private Vector3 _defaultRotationOffset;
    [SerializeField] private AnimationCurve _rotationAnimationCurve;
    [SerializeField] private float _animationTurnAngle;
    [SerializeField] private float _animationDuration;
    #endregion

    #region [ Unserialised Fields ]
    private Transform _transform;
    #endregion


    private void Awake() => _transform = transform;
    private void Start() => StartCoroutine(RotationAnimationCoroutine());

    private IEnumerator RotationAnimationCoroutine()
    {
        while (true)
        {
            yield return null;
            for (float timeElapsed = 0; timeElapsed < _animationDuration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, _animationDuration, timeElapsed);
                float evaluatedWeight = _rotationAnimationCurve.Evaluate(weight);

                Quaternion rotation = Quaternion.Euler(0, Mathf.Lerp(-_animationTurnAngle, _animationTurnAngle, evaluatedWeight), 0);
                _transform.rotation = Quaternion.Euler(_defaultRotationOffset) * rotation;
                yield return null;
            }
            
            yield return null;
            for (float timeElapsed = 0; timeElapsed < _animationDuration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, _animationDuration, timeElapsed);
                float evaluatedWeight = _rotationAnimationCurve.Evaluate(weight);

                Quaternion rotation = Quaternion.Euler(0, Mathf.Lerp(_animationTurnAngle, -_animationTurnAngle, evaluatedWeight), 0);
                _transform.rotation = Quaternion.Euler(_defaultRotationOffset) * rotation;
                yield return null;
            }
        }
    }
}
