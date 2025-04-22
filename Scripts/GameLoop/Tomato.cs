using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Tomato : MonoBehaviour
{
    private GameObject _tomatoSplatPrefab;

    [SerializeField] private float _splatDuration = 0.3f;
    [SerializeField] private Vector2 _splatScale = new Vector2(0.5f, 0.5f);
    [SerializeField] private float _projectScale = 0.15f;

    private bool _splatted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _tomatoSplatPrefab = Resources.Load<GameObject>("Prefabs/TomatoSplat");
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_splatted) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AchievementManager.AchievementEventRepository.OnTomato.Invoke();
            return;
        }

        AudioManager.Instance.PlayOneShotSplat(transform.position);
        
        _splatted = true;
        var hitTransform = other.gameObject.transform;

        var contact = other.GetContact(0);
        
        Vector3 normal = contact.normal;
        Vector3 spawnPosition = contact.point + (normal * 0.1f);
        
        Quaternion baseRotation = Quaternion.LookRotation(-normal);
        
        float randomAngle = UnityEngine.Random.Range(0f, 360f);
        Quaternion randomSpin = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        
        Quaternion finalRotation = baseRotation * randomSpin;

        var splat = GameObject.Instantiate(_tomatoSplatPrefab, spawnPosition, finalRotation);
        splat.transform.SetParent(hitTransform, true);

        var decalProj = splat.GetComponent<DecalProjector>();
        decalProj.size = new Vector3(0, 0, _projectScale);
        
        LeanTween.value(splat, 0f, 1f, _splatDuration)
            .setEaseOutBack()
            .setOnUpdate((float t) =>
            {
                decalProj.size = new Vector3(_splatScale.x * t, _splatScale.y * t, _projectScale);
            });
        
        GameObject.Destroy(this.gameObject);
    }
}
