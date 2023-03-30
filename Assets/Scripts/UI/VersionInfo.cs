using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.UI
{
    public class VersionInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionInfo;
        private AccessibilityManager abltManager;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(versionInfo);

#endif
            abltManager = GameObject.FindGameObjectWithTag("AccessibilityManager").GetComponent<AccessibilityManager>();
            if (abltManager.EnableInteractableHighlightsWhenHeld)
            {
                versionInfo.text = ("MAP B" + System.Environment.NewLine + "RC6");
            }
            else
            {
                versionInfo.text = ("MAP A" + System.Environment.NewLine + "RC6");
            }
            
        }

       
    }
}
