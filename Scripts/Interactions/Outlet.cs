using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Outlet : MonoBehaviour
{
    private enum ChargeIndicator
    {
        Green,
        Yellow,
        Red
    }

    private Vector3 _pluggedInWorldSpace = new Vector3(-0.016998291015625f, 1.0449999570846558f,9.897003173828125f);
    
    private ChargeIndicator currentCharge = ChargeIndicator.Red;
    
    [SerializeField] private GameObject redIndicator;
    private Material redMaterial;
    private Renderer redRenderer;
    [SerializeField] private GameObject yellowIndicator;
    private Material yellowMaterial;
    private Renderer yellowRenderer;
    [SerializeField] private GameObject greenIndicator;
    private Material greenMaterial;
    private Renderer greenRenderer;

    [SerializeField] private float chargingTime = 30f;
    private float chargingCounter = 0f;
    
    private bool done;

    private Material currentMaterial;
    private Renderer currentRenderer;

    private void Start()
    {
        redRenderer = redIndicator.GetComponent<Renderer>();
        redMaterial = redRenderer.material;

        yellowRenderer = yellowIndicator.GetComponent<Renderer>();
        yellowMaterial = yellowRenderer.material;

        greenRenderer = greenIndicator.GetComponent<Renderer>();
        greenMaterial = greenRenderer.material;
        
        greenMaterial.DisableKeyword("_EMISSION");
        yellowMaterial.DisableKeyword("_EMISSION");
        redMaterial.DisableKeyword("_EMISSION");

        // for testing
        // currentMaterial = redMaterial;
        // currentRenderer = redRenderer;
    }

    private bool blinkOn;
    private float blinkTimer;
    private void Update()
    {
        if (currentMaterial == null) return;

        if (blinkTimer <= 0)
        {
            blinkTimer = 1;

            blinkOn = !blinkOn;

            if (blinkOn) currentMaterial.EnableKeyword("_EMISSION");
            else currentMaterial.DisableKeyword("_EMISSION");

            RendererExtensions.UpdateGIMaterials(currentRenderer);
        }

        blinkTimer -= Time.deltaTime;
        chargingCounter += Time.deltaTime;

        if (chargingCounter >= chargingTime)
        {
            chargingCounter = 0;
            switch (currentCharge)
            {
                case ChargeIndicator.Red:
                    currentMaterial.EnableKeyword("_EMISSION");
                    currentMaterial = yellowMaterial;
                    currentRenderer = yellowRenderer;
                    currentCharge = ChargeIndicator.Yellow;
                    break;
                case ChargeIndicator.Yellow:
                    currentMaterial.EnableKeyword("_EMISSION");
                    currentMaterial = greenMaterial;
                    currentRenderer = greenRenderer;
                    currentCharge = ChargeIndicator.Green;
                    break;
                case ChargeIndicator.Green:
                    currentMaterial.EnableKeyword("_EMISSION");
                    currentMaterial = null;
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (done) return;

        if (other.CompareTag("Interactable"))
        {
            done = true;
            CameraInteractor.Instance.Drop();
            
            var rigidBoyd = other.GetComponent<Rigidbody>();
            rigidBoyd.useGravity = false;
            rigidBoyd.isKinematic = true;

            rigidBoyd.transform.position = _pluggedInWorldSpace;

            other.GetComponent<Holdable>().CanInteract = false;
            OnPluggedIn.Invoke();

            currentMaterial = redMaterial;
            currentRenderer = redRenderer;
        }
    }

    public UnityEvent OnPluggedIn = new UnityEvent();
}
