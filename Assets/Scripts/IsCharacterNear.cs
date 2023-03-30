using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Undercooked
{
    public class IsCharacterNear : MonoBehaviour
    {
        public Material originalMat, darkMat;
        private Transform requiredTransform;
        private GameObject playerObject1;
        private GameObject playerObject2;
        private AccessibilityManager abltManager;
        [SerializeField] private float distanceRequired = 3;
        bool darkTexture, lightTexture;
        public bool isStove = false;

        
        void Start()
        {
            abltManager = GameObject.FindGameObjectWithTag("AccessibilityManager").GetComponent<AccessibilityManager>();
            playerObject1 = GameObject.FindGameObjectWithTag("PlayerCooked");
            playerObject2 = GameObject.FindGameObjectWithTag("PlayerCooked2");
            if (abltManager.EnableInteractableHighlightsWhenHeld)
            {
                GetComponent<Renderer>().material = darkMat;
                darkTexture = true;
                lightTexture = false;
            }

            
        }

        // Update is called once per frame
        void Update()
        {
            if (abltManager.EnableInteractableHighlightsWhenHeld)
            {
                if (abltManager.isFirstPlayer)
                {
                    requiredTransform = playerObject1.transform;
                }
                else
                {
                    requiredTransform = playerObject2.transform;
                }
                if (isStove)
                {
                    if (transform.GetChild(0).childCount != 0)
                    {
                        if (transform.GetChild(0).transform.GetChild(0).name != "Plate1" &&
                            transform.GetChild(0).transform.GetChild(0).name != "Plate2" &&
                            transform.GetChild(0).transform.GetChild(0).name != "Plate3")
                        {
                            LightTextures();
                        }
                        else
                        if (transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).tag != "CleanPlates")
                        {
                            LightTextures();
                        }
                        else
                        if (abltManager.platesEnabled)
                        {
                            LightTextures();
                        }
                        else
                        {
                            DarkTextures();
                        }
                    }
                    else
                    {
                        if (abltManager.isShortDistanceCounters)
                        {
                            ChangeTextures();
                        }
                        else
                        {
                            DarkTextures();
                        }
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
                        DarkTextures();
                    }
                }
            }
            
        }

        void ChangeTextures()
        {
            float distance = Vector3.Distance(transform.position, requiredTransform.position);
            if ((distance < distanceRequired-0.5f) && darkTexture)
            {
                LightTextures();
            }

            if ((distance > distanceRequired-0.5f) && lightTexture)
            {
                DarkTextures();
            }
        }

        void LightTextures()
        {
            GetComponent<Renderer>().material = originalMat;
            darkTexture = false;
            lightTexture = true;
        }

        void DarkTextures()
        {
            GetComponent<Renderer>().material = darkMat;
            darkTexture = true;
            lightTexture = false;
        }
    }

}
