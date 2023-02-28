using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Undercooked
{
    public class IsCharacterNear : MonoBehaviour
    {
        public Material originalMat, darkMat;
        private GameObject playerObject;
        private AccessibilityManager abltManager;
        [SerializeField] private float distanceRequired = 3;
        bool darkTexture, lightTexture;
        public bool isStove = false;
        
        void Start()
        {
            abltManager = GameObject.FindGameObjectWithTag("AccessibilityManager").GetComponent<AccessibilityManager>();
            playerObject = GameObject.FindGameObjectWithTag("PlayerCooked");
            GetComponent<Renderer>().material = darkMat;
            darkTexture = true;
            lightTexture = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isStove)
            {
                if (abltManager.isShortDistanceCounters)
                {
                    ChangeTextures();
                }
                else
                {
                    LightTextures();
                }
            }
            else
            {
                if (abltManager.isShortDistanceWalls)
                {
                    ChangeTextures();
                }
                else
                {
                    LightTextures();
                }
            }
        }

        void ChangeTextures()
        {
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);
            if ((distance < distanceRequired) && darkTexture)
            {
                GetComponent<Renderer>().material = originalMat;
                darkTexture = false;
                lightTexture = true;
            }

            if ((distance > distanceRequired) && lightTexture)
            {
                GetComponent<Renderer>().material = darkMat;
                darkTexture = true;
                lightTexture = false;
            }
        }

        void LightTextures()
        {
            GetComponent<Renderer>().material = darkMat;
            darkTexture = true;
            lightTexture = false;
        }
    }

}
