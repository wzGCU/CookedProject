using Lean.Transition;
using UnityEngine;
using TMPro;
using Undercooked.Appliances;
using Undercooked.Managers;
using Undercooked.Model;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DeliverCountertopUI : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private static TextMeshProUGUI _text;

        [Header("notification colors")]
        [SerializeField] private Color positiveColorOutline;
        [SerializeField] private Color positiveColorBase;
        [SerializeField] private Color negativeColorOutline;
        [SerializeField] private Color negativeColorBase;
        
        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            #if UNITY_EDITOR
                Assert.IsNotNull(_text);
                Assert.IsNotNull(_canvasGroup);
            #endif
        }
        
        private void OnEnable()
        {
            OrderManager.OnOrderDelivered += HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing += HandlePlateMissing;
        }

        private void OnDisable()
        {
            OrderManager.OnOrderDelivered -= HandleOrderDelivered;
            DeliverCountertop.OnPlateMissing -= HandlePlateMissing;
        }

        private void HandlePlateMissing()
        {
            ScrollAndFadeText("NEEDS PLATE!", negativeColorBase, negativeColorOutline, 2f);
        }

        private void HandleOrderDelivered(Order order, int tip)
        {
            if (tip == 0) return;
            ScrollAndFadeText($"+{tip} TIP!", positiveColorBase, positiveColorOutline, 2f);
        }

        private void ScrollAndFadeText(string textToDisplay, Color baseColor, Color outlineColor, float timeToDisplayInSeconds = 2f)
        {
            _text.gameObject.transform.localPosition = Vector3.zero;
            _canvasGroup.alpha = 1f;
            _text.text = textToDisplay;
            _text.color = baseColor;
            _text.outlineColor = outlineColor;
            _canvasGroup.alphaTransition(0f, timeToDisplayInSeconds, LeanEase.Smooth);
            _text.rectTransform
                .localPositionTransition_Y(100f, timeToDisplayInSeconds, LeanEase.Smooth);
        }
        
    }
}
