using System.Threading.Tasks;
using Lean.Transition;
using UnityEngine;

namespace Undercooked.Utils
{
    public class MoveDollyTarget : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] private float deltaX = 9f;
        [SerializeField] private float oscillationDuration = 11f;
        private bool _oscillate;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            _oscillate = true;
            StartLerp();
        }

        private void OnDisable()
        {
            _oscillate = false;
        }

        private async void StartLerp()
        {
            while (_oscillate)
            {
                _transform
                    .positionTransition_X(deltaX, oscillationDuration, LeanEase.Smooth)
                    .JoinTransition()
                    .positionTransition_X(-deltaX, oscillationDuration, LeanEase.Smooth);
            
                await Task.Delay((int)oscillationDuration * 1000 * 2);   
            }
        }

    }
}
