using System.Collections;
using System.Collections.Generic;
using TMPro;
using Undercooked.Appliances;
using Undercooked.Data;
using Undercooked.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Undercooked
{
    
    public class AccessibilityManager : MonoBehaviour
    {

        public bool isFirstPlayer=false;

        public bool platesEnabled=false;
        private float fixedDeltaTime;
        [Header("Game Speed")]
        public bool isSlomo = true;
        private float gameSpeed = 0.85f;

        

        [Header("Highlights")]
        //Highlight Interactables
        public bool EnableInteractableHighlightsWhenHeld = true;
        [SerializeField] private Material originalCounterMat, 
            darkCounterMat, 
            originalCuttingMat, 
            darkCuttingMat,
            originalPot,
            darkHobPanMat,
            darkPot,
            originalTrash,
            darkTrash,
            originalDishTray,
            darkDishTray,
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
        public GameObject dishTray;
        public GameObject deliverCounter;
        public GameObject sink;
        public TextMeshProUGUI slowedText;
        public Image controls;
        public Sprite GamepadControlsSprite;
        public Sprite KeyboardControlsSprite;
        public TextMeshProUGUI startText;
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
            if (EnableInteractableHighlightsWhenHeld)
            {
                DisableAllButTaken();
                DisableSingular();
                
                if (isSlomo)
                {
                    ChangeSpeed();
                }
            }
               
            
        }
        private void FixedUpdate()
        {
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            
        }
        public void ChangeSpeed()
        {
            Debug.Log("Changed speed to: "+ gameSpeed);
            Time.timeScale = gameSpeed;
            //if (Time.timeScale == 1.0f)
            //{
            //    Debug.Log(slowedText.name);
            //    slowedText.enabled = true;
            //    Time.timeScale = gameSpeed;
            //    Debug.Log("Change Speed to: " + Time.timeScale);

            //}
            //else
            //    Time.timeScale = 1.0f;
            //slowedText.enabled = false;
            //Debug.Log("Change Speed to: " + Time.timeScale);
        }

         public void HandleIngredient(Ingredient data)
            {
            EnableHighlightCounters();
            EnableHighlightTrash();
            

            if (data.Status == IngredientStatus.Raw)
                {
                    EnableHighlightCuttingBoard();
                    DisableHighlightCooking();
                    DisableHighlightPlates();
                    DisableHighlightCooking();

                }
                if (data.Status == IngredientStatus.Processed)
                {
                    DisableHighlightCuttingBoards();
                    EnableHighlightCooking();
                    if (hobPan.transform.parent.GetComponent<CookingPot>().IsCookFinished)
                    {
                    
                    DisableHighlightCooking();
                    EnableHighlightPlates();
                }

                }
                
        }

        public void CheckIfOtherPlayerHasSomething()
        {
            DisableHighlightCounters();
            GameObject focusedPlayer;
            if (isFirstPlayer)
            {
                focusedPlayer = GameObject.FindGameObjectWithTag("PlayerCooked");
                
            }
            else
            {
                focusedPlayer = GameObject.FindGameObjectWithTag("PlayerCooked2");
            }

            if (focusedPlayer.transform.GetChild(1).childCount != 0)
            {
                Ingredient outputIngredient = focusedPlayer.transform.GetChild(1).transform.GetChild(0).GetComponent<Ingredient>();
                if (outputIngredient != null) {
                    HandleIngredient(outputIngredient);
                    EnableHighlightCounters();
                }
            }
        }

        public void UpdateControlsVisuals(PlayerInput input)
        {
            if (input.currentControlScheme == "Gamepad"){
                controls.sprite = GamepadControlsSprite;
                startText.text = "Press START to play";
            }
            else
            {
                controls.sprite = KeyboardControlsSprite;
                startText.text = "Press ENTER to play";
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
        public void DisableSpecificCuttingBoards(GameObject me)
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
              me.GetComponent<Renderer>().material = darkCounterMat;
              me.transform.GetChild(1).GetComponent<Renderer>().material = darkCuttingMat;
            
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
                    hobPan.GetComponent<Renderer>().material = darkHobPanMat;
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
                if ((dirtyplates.Length == 0))
                {
                    sink.GetComponent<Renderer>().material = darkSink;
                }
                
            }
        }

        public void EnableHighlightPlates()
        {
            
            if (EnableInteractableHighlightsWhenHeld)
            {
                platesEnabled = true;
                plates = GameObject.FindGameObjectsWithTag("CleanPlates");
                foreach (GameObject platePrefab in plates)
                {
                    Debug.Log("Enabling highlight of plate" + platePrefab.transform.parent.name);
                    platePrefab.GetComponent<Renderer>().material = originalPlate;
                }
                EnableHighlightCounters();
                DisableHighlightCounters();
            }

        }
        public void DisableHighlightPlates()
        {
            if (EnableInteractableHighlightsWhenHeld && !hobPan.transform.parent.GetComponent<CookingPot>().IsCookFinished)
            {
                
                platesEnabled = false;
                EnableHighlightCounters();
                DisableHighlightCounters();
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

        public void ForceDisableHighlightPlates()
        {
            
            if (EnableInteractableHighlightsWhenHeld)
            {
                plates = GameObject.FindGameObjectsWithTag("CleanPlates");
                foreach (GameObject platePrefab in plates)
                {
                    Debug.Log("printing "+ platePrefab.transform.parent.name);
                        platePrefab.GetComponent<Renderer>().material = darkPlate;

                    



                }
            }
        }

        public void EnableSpecificPlate(Renderer renderer)
        {
            //renderer.material = originalPlate;
        }

        public void EnableHighlightDishTray()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                dishTray.GetComponent<Renderer>().material = originalDishTray;
            }
            
        }
        public void DisableHighlightDishTray()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                dishTray.GetComponent<Renderer>().material = darkDishTray;
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
            DisableHighlightTrash();
            DisableHighlightCooking();
            
            
        }

        public void DisableSingular()
        {
            DisableHighlightSink();
            DisableHighlghtFoodCounter();
            DisableHighlightDishTray();
            ForceDisableHighlightPlates();
            
        }
       
        
      
        
    }

}

    
