using System.Collections;
using System.Collections.Generic;
using Undercooked.Model;
using Undercooked.Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Undercooked.Appliances
{
    // -- Particular Features --
    // we can drop a pile of plates into Sink
    // when player breaks contact, cleaning process is paused (it could be resumed)
    // when one plate is cleaned the next one starts automatically
    
    public class Sink : Interactable
    {
        [SerializeField] private Slider slider;
        [SerializeField] private List<Transform> dirtySlots = new List<Transform>();

        private readonly Stack<Plate> _cleanPlates = new Stack<Plate>();
        private readonly Stack<Plate> _dirtyPlates = new Stack<Plate>();

        private const float CleaningTime = 3f;
        private float _currentCleaningTime;
        private Coroutine _cleanCoroutine;

        public delegate void CleanStatus(PlayerController playerController);
        public static event CleanStatus OnCleanStart;
        public static event CleanStatus OnCleanStop;
        
        protected override void Awake()
        {
            base.Awake();
            
            #if UNITY_EDITOR
                Assert.IsNotNull(dirtySlots);
                Assert.IsNotNull(_cleanPlates);
                Assert.IsNotNull(_dirtyPlates);
                Assert.IsNotNull(slider);
            #endif
        }

        public override bool TryToDropIntoSlot(IPickable pickableToDrop)
        {
            if (!(pickableToDrop is Plate plate)) return false;
            if (!plate.IsEmpty() || plate.IsClean) return false;
            AddPileDirtyPlatesRecursively(plate);
            return true;
        }

        /// <summary>
        /// The first two dirty plate slots are visible, all subsequent are placed out of player's sight
        /// </summary>
        private void AddPileDirtyPlatesRecursively(Plate plate)
        {
            Plate nextPlate = plate.Slot.GetComponentInChildren<Plate>();
            if (nextPlate != null)
            {
                nextPlate.transform.SetParent(null);
                AddPileDirtyPlatesRecursively(nextPlate);
            }
            
            _dirtyPlates.Push(plate);
            int dirtySize = _dirtyPlates.Count;
            
            Transform dirtySlot = dirtySize <= 2 ? dirtySlots[dirtySize - 1] : dirtySlots[2];
            
            plate.transform.SetParent(dirtySlot);
            plate.transform.SetPositionAndRotation(dirtySlot.transform.position, dirtySlot.transform.rotation);
        }

        public override IPickable TryToPickUpFromSlot(IPickable playerHoldPickable)
        {
            if (playerHoldPickable != null) return null;
            
            return _cleanPlates.Count > 0 ? _cleanPlates.Pop() : null;
        }

        public override void Interact(PlayerController playerController)
        {
            base.Interact(playerController);

            if (_dirtyPlates.Count == 0) return;
            
            if (_cleanCoroutine == null)
            {
                _currentCleaningTime = 0f;
                slider.value = 0f;
                slider.gameObject.SetActive(true);
                StartCleanCoroutine();
                return;
            }
            
            StopCleanCoroutine();
            StartCleanCoroutine();
        }
        
        private IEnumerator Clean()
        {
            slider.gameObject.SetActive(true);
            while (_currentCleaningTime < CleaningTime)
            {
                slider.value = _currentCleaningTime / CleaningTime;
                _currentCleaningTime += Time.deltaTime;
                yield return null;
            }
            
            // clean the top of the _dirtyPlates stack
            var plateToClean = _dirtyPlates.Pop();
            plateToClean.SetClean();
            
            // put the clean plate into the top of the cleanPile (physically)
            var topStackSlot = _cleanPlates.Count == 0 ? Slot : _cleanPlates.Peek().Slot;
            
            // all plates parented to the base Slot, but physically positioned on the top of the stack
            plateToClean.transform.SetParent(Slot);
            plateToClean.transform.SetPositionAndRotation(topStackSlot.transform.position, Quaternion.identity);

            _cleanPlates.Push(plateToClean);
            
            _cleanCoroutine = null;
            _currentCleaningTime = 0f;

            // Chain next plate
            if (_dirtyPlates.Count > 0)
            {
                StartCleanCoroutine();
                yield break;
            }
            
            StopCleanCoroutine();
        }
        
        public override void ToggleHighlightOff()
        {
            base.ToggleHighlightOff();
            StopCleanCoroutine();
        }
        
        private void StartCleanCoroutine()
        {
            OnCleanStart?.Invoke(LastPlayerControllerInteracting);
            _cleanCoroutine = StartCoroutine(Clean());
        }
        
        private void StopCleanCoroutine()
        {
            OnCleanStop?.Invoke(LastPlayerControllerInteracting);
            slider.gameObject.SetActive(false);
            if (_cleanCoroutine != null) StopCoroutine(_cleanCoroutine);
        }
    }
}
