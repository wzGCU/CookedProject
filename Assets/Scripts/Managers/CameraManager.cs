using Cinemachine;
using Lean.Transition;
using Undercooked.Player;
using UnityEngine;
using UnityEngine.Assertions;
using static Undercooked.Player.InputController;

namespace Undercooked.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera1;
        [SerializeField] private CinemachineVirtualCamera virtualCamera2;
        [SerializeField] private CinemachineVirtualCamera virtualCamera4;
        [SerializeField] private CinemachineVirtualCamera dollyCamera;
        [SerializeField] private Transform dollyCameraTarget;

        private const float TransitionDelayInSeconds = 0.75f;
        [SerializeField] private ParticleSystem starGlowParticleSystem;

        private Transform _particleSystemTransform;
        private Transform _avatar1;
        private Transform _avatar2;
        private InputController.PlayerControllerIndex _lastActivatedPlayerController;

        private AccessibilityManager abltManager;
        
        private void Awake()
        {
            #if UNITY_EDITOR
                Assert.IsNotNull(virtualCamera1);
                Assert.IsNotNull(virtualCamera2);
                Assert.IsNotNull(dollyCamera);
                Assert.IsNotNull(starGlowParticleSystem);
                Assert.IsNotNull(dollyCameraTarget);
            #endif
            
            starGlowParticleSystem.Play();
            _particleSystemTransform = starGlowParticleSystem.transform;
            _avatar1 = virtualCamera1.Follow;
            _avatar2 = virtualCamera2.Follow;
            _particleSystemTransform.position = _avatar1.position;
            abltManager = GameObject.FindGameObjectWithTag("AccessibilityManager").GetComponent<AccessibilityManager>();
        }

        public void FocusFirstPlayer()
        {
            if(!abltManager.EnableInteractableHighlightsWhenHeld)
            SwitchFocus(InputController.PlayerControllerIndex.First);
            else
            {
                SetCameraAccessible();
            }
        }

        private void OnEnable()
        {
            InputController.OnSwitchPlayerController += HandleSwitchPlayerController;
            
        }

        private void OnDisable()
        {
            InputController.OnSwitchPlayerController -= HandleSwitchPlayerController;
        }

        private void HandleSwitchPlayerController(InputController.PlayerControllerIndex playerControllerIndex)
        {
            if(!abltManager.EnableInteractableHighlightsWhenHeld)
           SwitchFocus(playerControllerIndex);
        }
        private void SetCameraAccessible()
        {
            dollyCamera.gameObject.SetActive(false);
            dollyCameraTarget.gameObject.SetActive(false);

            
           virtualCamera1.gameObject.SetActive(false);
           virtualCamera2.gameObject.SetActive(false);
            virtualCamera4.gameObject.SetActive(true);
               
            
            
        }
        private void SwitchFocus(InputController.PlayerControllerIndex playerControllerIndex)
        {
            dollyCamera.gameObject.SetActive(false);
            dollyCameraTarget.gameObject.SetActive(false);
            
            switch (playerControllerIndex)
            {
                case InputController.PlayerControllerIndex.First:
                    if (_lastActivatedPlayerController != InputController.PlayerControllerIndex.None)
                    {
                        MoveStarParticle(_avatar2, _avatar1, TransitionDelayInSeconds);
                    }
                    virtualCamera1.gameObject.SetActive(true);
                    virtualCamera2.gameObject.SetActive(false);
                    break;
                case InputController.PlayerControllerIndex.Second:
                    if (_lastActivatedPlayerController != InputController.PlayerControllerIndex.None)
                    {
                        MoveStarParticle(_avatar1, _avatar2, TransitionDelayInSeconds);
                    }
                    virtualCamera1.gameObject.SetActive(false);
                    virtualCamera2.gameObject.SetActive(true);
                    break;
                default:
                    Debug.LogWarning("[CameraManager] Unexpected player controller index", this);
                    break;
            }
            _lastActivatedPlayerController = playerControllerIndex;
        }

        internal void SwitchDollyCamera()
        {
            if (!abltManager.EnableInteractableHighlightsWhenHeld)
            {
                _lastActivatedPlayerController = InputController.PlayerControllerIndex.None;

                virtualCamera1.gameObject.SetActive(false);
                virtualCamera2.gameObject.SetActive(false);
                dollyCamera.gameObject.SetActive(true);
                dollyCameraTarget.gameObject.SetActive(true);
            }
            
        }
        
        private void MoveStarParticle(Transform from, Transform to, float delayInSeconds = 1f)
        {
            if (!abltManager.EnableInteractableHighlightsWhenHeld)
            {
                starGlowParticleSystem.Play();
                _particleSystemTransform.position = from.position;
                _particleSystemTransform
                    .positionTransition(to.position, delayInSeconds, LeanEase.Smooth);
            }
            
        }
    }
}
//Vector3(0.0500000007,6.92999983,-5.9000001)
//Vector3(0.0500000007,7.78999996,-6.38000011)