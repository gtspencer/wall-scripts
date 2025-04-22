using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayPaint : Holdable
{
    private AudioSource _sfx;
    private bool _painting;
    
    [SerializeField] private float _maxPaintDistance = 10;
    [SerializeField] private LayerMask _paintableLayers;
    [SerializeField] private GameObject _brush;
    
    protected override void OnStart()
    {
        _sfx = GetComponentInChildren<AudioSource>();
        base.OnStart();
    }

    protected override void OnHovered()
    {
        
    }

    protected override void OnUnHovered()
    {
        
    }

    protected override void OnHeld()
    {
        StartCoroutine(BriefWaitBeforeSpray());
    }

    // used to not spray paint/make noise the couple frames after picking up
    private IEnumerator BriefWaitBeforeSpray()
    {
        yield return new WaitForSeconds(0.1f);
        
        InputRepository.Instance.OnFireStarted += StartPaint;
        InputRepository.Instance.OnFireStopped += StopPaint;
    }

    protected override void OnDropped()
    {
        InputRepository.Instance.OnFireStarted -= StartPaint;
        InputRepository.Instance.OnFireStopped -= StopPaint;
    }

    private void FixedUpdate()
    {
        if (!_painting) return;
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxPaintDistance, _paintableLayers))
        {
            if (hit.collider.isTrigger) return;

            var position = hit.point + hit.normal * 0.05f;
            
            /*
            Vector3 normal = hit.normal.normalized;

            if (normal == Vector3.zero)
                normal = Vector3.up;
            
            Vector3 forward = Vector3.Cross(normal, Vector3.up);
            if (forward == Vector3.zero) // Happens if normal is parallel to Vector3.right
                forward = Vector3.Cross(normal, Vector3.forward);*/
            
            var temp = Instantiate(_brush, position, Quaternion.LookRotation(-hit.normal));
        }
    }

    private void StartPaint()
    {
        _sfx.Play();
        _painting = true;
    }

    private void StopPaint()
    {
        _sfx.Stop();
        _painting = false;
    }
}
