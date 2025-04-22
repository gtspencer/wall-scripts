using UnityEngine;

namespace Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        protected Collider _collider;
        
        public bool CanInteract = true;
        
        [SerializeField] private Color outlineColor = Color.yellow;

        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            gameObject.tag = "Interactable";

            _collider = gameObject.GetComponent<Collider>();

            OnStart();
        }
        
        protected abstract void OnStart();
        
        protected abstract void OnInteracted();
        public void Interact()
        {
            OnInteracted();
        }
        
        protected abstract void OnHovered();
        public void Hovered()
        {
            // _outline.enabled = true;
            OnHovered();
        }
    
        protected abstract void OnUnHovered();
        public void UnHovered()
        {
            // _outline.enabled = false;
            OnUnHovered();
        }
    }
}