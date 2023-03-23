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
        //Game Speed
        private float fixedDeltaTime;
        public bool isSlomo = true;
        public float gameSpeed = 0.75f;
        private GameObject[] cuttingBoards;

        public Material originalCounterMat, darkCounterMat, originalCuttingMat, darkCuttingMat;

        public bool isShortDistanceWalls = true;
        public bool isShortDistanceCounters = true;
        



        //Highlight Interactables
        public bool EnableInteractableHighlightsWhenHeld = true;

        private void Awake()
        {
            this.fixedDeltaTime = Time.fixedDeltaTime;
            cuttingBoards = GameObject.FindGameObjectsWithTag("CuttingBoards");
            DisableHighlightCuttingBoards();
            
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
            }
            else
                Time.timeScale = 1.0f;
            Debug.Log("Change Speed to: " + Time.timeScale);
        }

     
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
        void EnableHighlightCuttingBoard()
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
        void DisableHighlightCuttingBoards()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {
                foreach (GameObject cuttingPrefab in cuttingBoards)
                {
                    if (!cuttingPrefab.GetComponent<ChoppingBoard>().IsBusy())
                    {
                        cuttingPrefab.GetComponent<Renderer>().material = darkCounterMat;
                        cuttingPrefab.transform.GetChild(1).GetComponent<Renderer>().material = darkCuttingMat;
                     }
                
                
                }
            }
                
            
        }

        void EnableHighlightCooking()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {

            }
        }

        void DisableHighlightCooking()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {

            }
        }

        void EnableHighhlightPlate()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {

            }
        }
        void DisableHighlightPlate()
        {
            if (EnableInteractableHighlightsWhenHeld)
            {

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

        public void HandleIngredient(Ingredient data)
        {
            
            if (data.Status == IngredientStatus.Raw)
            {
                EnableHighlightCuttingBoard();
                DisableHighlightCooking();
                DisableHighlightPlate();

            }
            if (data.Status==IngredientStatus.Processed)
            {
                DisableHighlightCuttingBoards();
                EnableHighlightCooking();
                if (data.Type == IngredientType.Lettuce) EnableHighhlightPlate();
                else DisableHighlightPlate();

            }
            if (data.Status == IngredientStatus.Cooked)
            {
                DisableHighlightCuttingBoards();
                DisableHighlightCooking();
                EnableHighhlightPlate();
            }
            
        }

        public void DisableAllButCrates()
        {

        }
       
        
      
        
    }

}

    
