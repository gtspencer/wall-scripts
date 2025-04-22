using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerHoldable : Holdable
{
    [SerializeField] private LayerMask wallLayer;

    private Camera _camera;

    private bool _stickerOnWall = false;
    public bool StickerOnWall => _stickerOnWall;

    protected override void OnStart()
    {
        base.OnStart();
        _camera = Camera.main;
    }

    protected override void OnHovered()
    {
    }

    protected override void OnUnHovered()
    {
    }

    protected override void OnHeld()
    {
    }

    protected override void OnDropped()
    {
        if (!_stickerOnWall) return;

        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHeld) return;
        
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out var hit, 5, wallLayer))
        {
            _stickerOnWall = true;
            transform.rotation = Quaternion.LookRotation(hit.normal);
            transform.position = hit.point + (transform.forward * 0.01f);
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _stickerOnWall = false;
        }
    }
}
