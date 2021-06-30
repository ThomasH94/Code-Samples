using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Helper enum for shapes
/// </summary>
public enum ShapeType
{
    NONE,
    X,
    HEX,
    DIAMOND,
    ARROW
}

/// <summary>
/// Helper enum for shape colors
/// </summary>
public enum ShapeColor
{
    NONE,
    RED,
    BLUE,
    YELLOW
}

/// <summary>
/// This class allows an object to be dragged by the mouse while the left mouse button is held down
/// </summary>
public class DraggableObject : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler, IAudible
{
    [Header("Shape Data")]
    /// <summary>
    /// Information about this shape
    /// </summary>
    public ShapeType shapeType;
    public ShapeColor shapeColor;

    [Header("UI")]
    /// <summary>
    /// References to the UI components
    /// </summary>
    private RectTransform rectTransform;
    private Canvas canvasParent;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    [Header("Tweening")]
    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeSpring;
    [SerializeField] private float tweenTime = 0.2f;

    public AudioPlayer audioPlayer { get; set; }

    // Set to an empty delegate to avoid null checks if desired
    public static event Action ShapeRemoved;
    public static event Action RegisterShape;

    /// <summary>
    /// Awake is being used to capture component references
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasParent = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioPlayer = GetComponent<AudioPlayer>();

        if (canvasParent == null) Debug.LogError($"<color=red>ERROR!</color> CANVAS NOT FOUND!");

        startPosition = rectTransform.anchoredPosition;
    }

    /// <summary>
    /// Start is being used to register this shape to ensure the objects listening have had their Awake methods called
    /// </summary>
    private void Start()
    {
        // Register with the Game Manager without a hard reference
        RegisterShape?.Invoke();
    }

    /// <summary>
    /// Method that allows the object to be dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // Set this objects position to the mouse by adjusting the mouse position based on the scale factor of the canvas
        rectTransform.anchoredPosition += eventData.delta / canvasParent.scaleFactor;
    }

    /// <summary>
    /// Method for handling the item being dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the raycasts to shoot through this object to hit the Shape Detectors
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Method for handling the item being let go
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Block raycasts when the object is let go
        canvasGroup.blocksRaycasts = true;
        ReturnToStartPosition();
    }

    /// <summary>
    /// Removes the shape from the total amount of shapes and destorys this gameobject
    /// </summary>
    public void RemoveShape()
    {
        ShapeRemoved?.Invoke();
        Destroy(gameObject);
    }

    /// <summary>
    /// Method to return the shape to it's starting position if let go before being destroyed
    /// </summary>
    public void ReturnToStartPosition()
    {
        LeanTween.move(rectTransform, startPosition, tweenTime).setEase(tweenType);
        audioPlayer.PlayAudioOneShot();
    }
}