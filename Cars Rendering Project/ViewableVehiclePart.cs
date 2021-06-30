using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace VehicleSystem
{
    public class ViewableVehiclePart : MonoBehaviour
    {
        #region PartInformation
        [Header("Part Information")]
        public Vector3 viewingAngle;
        public Vector3 viewPosition;    // The position the camera should move too
        public string partName;
        public GameObject vehiclePartLabel;
        public TextMeshPro vehiclePartLabelText;
        #endregion

        #region Rendering
        [Header("Rendering")]
        private Renderer vehiclePartRenderer;
        private Material defaultMaterial;
        [SerializeField] private Material glowMaterial;
        private Material currentMaterial;
        #endregion

        #region Events
        [Header("Events")]
        public GameObjectEvent moveToObjectEvent;
        #endregion

        /// <summary>
        /// Start is used to create and set the information about this vehicle part
        /// </summary>
        private void Start()
        {
            GenerateVehicleName();
            SetPartRendering();
            ToggleLabel(false);
        }

        /// <summary>
        /// Sets the Vehicle Name based on the gameobjects name if empty
        /// </summary>
        public void GenerateVehicleName()
        {
            if(partName == "" || partName == string.Empty) partName = gameObject.name;
            vehiclePartLabelText.text = partName;
        }

        /// <summary>
        /// Gets the parts renderer information
        /// </summary>
        private void SetPartRendering()
        {
            vehiclePartRenderer = GetComponent<Renderer>();
            defaultMaterial = vehiclePartRenderer.material;;
        }

        /// <summary>
        /// Highlights the selected part using it's highlighted material
        /// </summary>
        private void OnMouseOver()
        {
            if (!IsPointerOverUIObject())
            {
                EnablePartHighlight(true);

                if (Input.GetButtonDown("Fire1"))
                {
                    MoveToVehicleInvoker();
                }
            }
        }

        /// <summary>
        /// Deselects the part and stops highlighting it
        /// </summary>
        private void OnMouseExit()
        {
            EnablePartHighlight(false);
        }

        /// <summary>
        /// Event Invoker for moving to vehicles
        /// </summary>
        public void MoveToVehicleInvoker()
        {
            moveToObjectEvent.Raise(gameObject);
        }

        /// <summary>
        /// Set the material to the glowing material if the mouse is hovering over the vehicle part
        /// </summary>
        /// <param name="enableHighlighting"></param>
        private void EnablePartHighlight(bool enableHighlighting)
        {
            if (vehiclePartRenderer != null && defaultMaterial != null && glowMaterial != null)
            {
                vehiclePartRenderer.material = enableHighlighting ? glowMaterial : defaultMaterial;
            }
        }

        /// <summary>
        /// Event invoker for toggling labels
        /// </summary>
        public void ToggleLabelInvoker()
        {
            ToggleLabel(true);
        }

        /// <summary>
        /// Enables/Disables toggle for labels on parts
        /// </summary>
        /// <param name="toggle"></param>
        public void ToggleLabel(bool toggle)
        {
            vehiclePartLabel.SetActive(toggle);
            Debug.Log("Label toggled? " + toggle);
        }

        /// <summary>
        /// Helper method to avoid lighting up when over the UI
        /// </summary>
        /// <returns></returns>
        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    }
}