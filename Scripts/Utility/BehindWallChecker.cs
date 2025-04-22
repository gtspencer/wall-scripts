using System;
using System.Collections;
using UnityEngine;

public class BehindWallChecker : MonoBehaviour
{
    [SerializeField] private Transform _pointA, _pointB, _pointC, _pointD; // The four points defining the plane
    [SerializeField] private Transform _target; // The object to check
    [SerializeField] private Transform _cameraTransform; // The camera
    private Camera _camera;

    private bool _started;
    private bool _done;

    private PlaySingleAnimation _animationPlayer;
    private Renderer _targetRenderer;

    [SerializeField] private GameObject _otherLimper;

    private void Start()
    {
        _camera = _cameraTransform.GetComponent<Camera>();
        _targetRenderer = _target.GetComponentInChildren<Renderer>();
        _animationPlayer = _target.GetComponentInChildren<PlaySingleAnimation>();
    }

    void Update()
    {
        if (!_started)
        {
            if (!_targetRenderer.isVisible) return;

            _otherLimper.SetActive(false);
            _started = true;
            _animationPlayer.enabled = true;
        }
        
        if (_done) return;
        
        if (IsTargetOccluded(_target.position))
        {
            _done = true;
            StartCoroutine(Disappear_Coroutine());
        }
    }

    private IEnumerator Disappear_Coroutine()
    {
        yield return new WaitForSeconds(.6f);

        _target.gameObject.SetActive(false);
    }

    bool IsTargetOccluded(Vector3 targetPosition)
    {
        // Step 1: Compute the plane normal
        Vector3 v1 = _pointB.position - _pointA.position;
        Vector3 v2 = _pointC.position - _pointA.position;
        Vector3 planeNormal = Vector3.Cross(v1, v2).normalized;

        // Step 2: Ensure normal faces the camera
        Vector3 cameraDirection = (_camera.transform.position - _pointA.position).normalized;
        if (Vector3.Dot(planeNormal, cameraDirection) < 0)
        {
            planeNormal = -planeNormal;
        }

        // Step 3: Check if the target is behind the plane
        Vector3 toTarget = targetPosition - _pointA.position;
        bool isBehind = Vector3.Dot(planeNormal, toTarget) < 0;
        if (!isBehind) return false; // If in front, it's not occluded

        // Step 4: Project the plane points & target into camera screen space
        Vector3[] screenPoints = new Vector3[4];
        screenPoints[0] = _camera.WorldToViewportPoint(_pointA.position);
        screenPoints[1] = _camera.WorldToViewportPoint(_pointB.position);
        screenPoints[2] = _camera.WorldToViewportPoint(_pointC.position);
        screenPoints[3] = _camera.WorldToViewportPoint(_pointD.position);
        Vector3 screenTarget = _camera.WorldToViewportPoint(targetPosition);

        // Step 5: Check if the target is inside the 2D plane quad
        return IsPointInQuad(screenTarget, screenPoints);
    }

    bool IsPointInQuad(Vector3 point, Vector3[] quad)
    {
        // Using a point-in-triangle method for a convex quad
        return IsPointInTriangle(point, quad[0], quad[1], quad[2]) ||
               IsPointInTriangle(point, quad[0], quad[2], quad[3]);
    }

    bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        float Area(Vector3 p1, Vector3 p2, Vector3 p3) =>
            Mathf.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2.0f);

        float fullArea = Area(a, b, c);
        float area1 = Area(p, b, c);
        float area2 = Area(a, p, c);
        float area3 = Area(a, b, p);

        return Mathf.Abs(fullArea - (area1 + area2 + area3)) < 0.0001f;
    }
}