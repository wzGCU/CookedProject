using System.Collections;
using System.Collections.Generic;
using Undercooked.Appliances;
using Undercooked.Data;
using Undercooked.Model;
using UnityEngine;

namespace Undercooked
{
    
    public class AccessibilityManager : MonoBehaviour
    {
        
        private float fixedDeltaTime;
        [Header("Game Speed")]
        public bool isSlomo = true;
        public float gameSpeed = 0.75f;

        

        [Header("Highlights")]
        //Highlight Interactables
        public bool EnableInteractableHighlightsWhenHeld = true;
        [SerializeField] private Material originalCounterMat, 
            darkCounterMat, 
            originalCuttingMat, 
            darkCuttingMat,
            originalPot,
            darkPot,
            originalTrash,
            darkTrash,
            originalSink,
            darkSink,
            originalFoodGiver,
            darkFoodGiver,
            originalPlate,
            darkPlate;

        [Header("Game Objects")]
        public GameObject hobCooker;
        public GameObject hobPan;
        public GameObject trash;
        public GameObject deliverCounter;
        public GameObject sink;
        public GameObject slowedText;
        private GameObject[] cuttingBoards;
        private GameObject[] plates;
        private GameObject[] fullplates;
        private GameObject[] dirtyplates;

        [Header("Variables for seeing when walls are on/off")]
        public bool isShortDistanceWalls = true;
        public bool isShortDistanceCounters = true;


        private void Awake()
        {
            this.fixedDeltaTime = Time.fixedDeltaTime;
            cuttingBoards = GameObject.FindGameObjectsWithTag("CuttingBoards");
            DisableAllButTaken();
            
        }
        private void FixedUpdate()
        {
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        public void ChangeSpeed()
        {

            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = gameSpeed;
                slowedText.SetActive(true);
            }
            else
                Time.timeScale = 1.0f;
            slowedText.SetActive(false);
            Debug.Log("Change Speed to: " + Time.timeScale);
        }

         public void HandleIngredient(Ingredient data)
            {
            EnableHighlightTrash();

            if (data.Status == IngredientStatus.Raw)
                {
                    EnableHighlightCuttingBoard();
                    DisableHighlightCooking();
                    DisableHighlightPlates();

                }
                if (data.Status == IngredientStatus.Processed)
                {
                    DisableHighlightCuttingBoards();
                    EnableHighlightCooking();
                if (data.Type == IngredientType.Lettuce) EnableHighlightPlates();
                else DisableHighlightPlates();

                }
                if (data.Status == IngredientStatus.Cooked)
                {
                    DisableHighlightCuttingBoards();
                    DisableHighlightCooking();
                    EnableHighlightPlates();
                }
            
            }

        //All interactables Higglight functions
        public void SwitchHighlightCuttingBoard(bool isHighlighted)
        {
            if (isHighlighted)
            {
                EnableHighlightCuttingBoard();
            }
            else
            {
                DisableHighlightCuttingBoards();
            }
        }
        public void EnableHighlightCuttingBoard()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                foreach (GameObject cuttingPrefab in cuttingBoards)
                {
                    cuttingPrefab.GetComponent<Renderer>().material = originalCounterMat;
                    cuttingPrefab.transform.GetChild(1).GetComponent<Renderer>().material = originalCuttingMat;
                
                }
            }
                
        }
        public void DisableHighlightCuttingBoards()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                foreach (GameObject cuttingPrefab in cuttingBoards)
                {
                    if (cuttingPrefab.transform.GetChild(0).childCount==0)
                    {
                        cuttingPrefab.GetComponent<Renderer>().material = darkCounterMat;
                        cuttingPrefab.transform.GetChild(1).GetComponent<Renderer>().material = darkCuttingMat;
                     }
                
                
                }
            }
                
            
        }

        public void EnableHighlightCounters()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                isShortDistanceCounters= true;
            }
        }
        public void DisableHighlightCounters()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                isShortDistanceCounters= false;
            }
        }
        
        public void EnableHighlightCooking()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                hobCooker.transform.GetChild(0).gameObject.SetActive(true);
                hobCooker.transform.GetChild(1).GetComponent<Renderer>().material = originalPot;
                hobPan.GetComponent<Renderer>().material = originalCuttingMat;
            }
        }
        public void DisableHighlightCooking()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                if (hobPan.transform.parent.GetComponent<CookingPot>().IsEmpty())
                {
                    hobCooker.transform.GetChild(0).gameObject.SetActive(false);
                    hobCooker.transform.GetChild(1).GetComponent<Renderer>().material = darkPot;
                    hobPan.GetComponent<Renderer>().material = darkCuttingMat;
                }
            }
        }

        public void EnableHighlightSink()
        {
            Debug.Log("enablingsink");
            if (EnableInteractableHighlightsWhenHeld)
            {
                sink.GetComponent<Renderer>().material = originalSink;
            }
        }
        public void DisableHighlightSink()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                dirtyplates = GameObject.FindGameObjectsWithTag("DirtyPlates");
                if (dirtyplates.Length == 0)
                {
                    sink.GetComponent<Renderer>().material = darkSink;
                }
                else
                {
                    foreach(GameObject plate in dirtyplates)
                    {
                        Debug.Log(plate.name);
                    }
                }
            }
        }

        public void EnableHighlightPlates()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                plates = GameObject.FindGameObjectsWithTag("CleanPlates");
                foreach (GameObject platePrefab in plates)
                {
                    platePrefab.GetComponent<Renderer>().material = originalPlate;
                }
            }

        }
        public void DisableHighlightPlates()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                plates = GameObject.FindGameObjectsWithTag("CleanPlates");
                foreach (GameObject platePrefab in plates)
                {
                    if (!platePrefab.transform.GetChild(1).gameObject.activeSelf)
                    {
                        platePrefab.GetComponent<Renderer>().material = darkPlate;
                        
                    }


                }
            }


        }

        public void EnableHighlightTrash()
        {
            if (EnableInteractableHighlightsWhenHeld)
            trash.GetComponent<Renderer>().material = originalTrash;
        }
        public void DisableHighlightTrash()
        {
            if (EnableInteractableHighlightsWhenHeld)
                trash.GetComponent<Renderer>().material = darkTrash;
        }

        public void EnableHighlghtFoodCounter()
        {
            
            if (EnableInteractableHighlightsWhenHeld)
            {
                Material[] newMaterials;
                newMaterials = deliverCounter.GetComponent<Renderer>().materials;
                newMaterials[1] = originalFoodGiver;
                deliverCounter.GetComponent<Renderer>().materials = newMaterials;
            }
                
        }
        public void DisableHighlghtFoodCounter()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                fullplates = GameObject.FindGameObjectsWithTag("FullPlates");
                if (fullplates.Length == 0)
                {
                    Material[] newMaterials;
                    newMaterials = deliverCounter.GetComponent<Renderer>().materials;
                    newMaterials[1] = darkFoodGiver;
                    deliverCounter.GetComponent<Renderer>().materials = newMaterials;
                }
                else
                {
                   
                }
            }
        }

        public void DisableAllButTaken()
        {
            DisableHighlightCuttingBoards();
            DisableHighlightCounters();
            DisableHighlightPlates();
            DisableHighlightCooking();
            DisableHighlightSink();
            DisableHighlightTrash();
            DisableHighlghtFoodCounter();
            
        }
       
        
      
        
    }

}

    
