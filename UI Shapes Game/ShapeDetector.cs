using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class will detect any shape overlaps and destroy the shape if it meets this objects destroy requirements 
/// </summary>
public class ShapeDetector : MonoBehaviour, IDropHandler, IAudible 
{
    [Header("Shape Info")]
    [SerializeField] private ShapeType desiredShape;
    [SerializeField] private ShapeColor desiredColor;
    [SerializeField] private bool checkColors;

    public AudioPlayer audioPlayer { get; set; }

    /// <summary>
    /// Awake is used to get component references
    /// </summary>
    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    /// <summary>
    /// Method to check when a draggable-object is dropped on this object
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the draggable object meets the shape/color requirement of this detector
        if(eventData.pointerDrag != null)
        {
            DraggableObject shape = eventData.pointerDrag.GetComponent<DraggableObject>();
            if (checkColors) CheckColor(shape);
            else CheckShape(shape);
        }
    }

    /// <summary>
    /// Method to check the shape to see if it matches the desires shape
    /// </summary>
    /// <param name="p_shape"></param>
    private void CheckShape(DraggableObject p_shape)
    {
        if (p_shape.shapeType == desiredShape)
        {
            audioPlayer.PlayAudioOneShot();
            p_shape.RemoveShape();
        }
    }

    /// <summary>
    /// Method to check the shape to see if it matches the desired color
    /// </summary>
    /// <param name="p_shape"></param>
    private void CheckColor(DraggableObject p_shape)
    {
        if (p_shape.shapeColor == desiredColor)
        {
            audioPlayer.PlayAudioOneShot();
            p_shape.RemoveShape();
        }
    }
}
