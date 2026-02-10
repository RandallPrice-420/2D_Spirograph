using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Spirograph : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   DelaySeconds
    //   NumberOfIterations
    //   NumberOfCircles
    //   SmallerCircleRadiusRatio
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    [Range(0.0f, 1.0f)] public float DelaySeconds       = 0.001f;
    [Range(0, 20000)]   public int   NumberOfIterations = 15000;

    [Range(_minNumberOfCircles, _maxNumberOfCircles)] public int   NumberOfCircles;
    [Range(_minCircleRatio,     _maxCircleRatio)]     public float SmallCircleRadiusRatio;

    #endregion



    // -------------------------------------------------------------------------
    // Private Serialize Variables:
    // ----------------------------
    //   _circlePrefab
    //   _penPointPrefab
    //   _circlesRatioSlider
    //   _circlesRatioValue
    //   _penPositionSlider
    //   _penPositionValue
    //   _penWidthSlider
    //   _penWidthValue
    //   _numberOfCirclesDropdown
    //   _drawButton
    //   _inputIterations
    //   _currentIterationValue
    //   _largeCircleMat
    //   _smallCirclesMat
    //   _largeCircleSprite
    //   _smallCircleSprite
    // -------------------------------------------------------------------------

    #region .  Private Serialize Variables  .

    [SerializeField] private GameObject     _circlePrefab;
    [SerializeField] private GameObject     _penPointPrefab;
    [SerializeField] private Slider         _circlesRatioSlider;
    [SerializeField] private TMP_Text       _circlesRatioValue;
    [SerializeField] private Slider         _penPositionSlider;
    [SerializeField] private TMP_Text       _penPositionValue;
    [SerializeField] private Slider         _penWidthSlider;
    [SerializeField] private TMP_Text       _penWidthValue;
    [SerializeField] private TMP_Dropdown   _numberOfCirclesDropdown;
    [SerializeField] private Button         _drawButton;
    [SerializeField] private TMP_InputField _inputIterations;
    [SerializeField] private TMP_Text       _currentIterationValue;
    [SerializeField] private Material       _largeCircleMat;
    [SerializeField] private Material       _smallCirclesMat;
    [SerializeField] private Sprite         _largeCircleSprite;
    [SerializeField] private Sprite         _smallCircleSprite;
    #endregion



    // -------------------------------------------------------------------------
    // Private Variables:
    // ------------------
    //   _minNumberOfCircles
    //   _maxNumberOfCircles
    //   _minCircleRatio
    //   _maxCircleRatio
    //
    //   _angleIncrement
    //   _batchSizeMin
    //   _batchSizeMax
    //   _largeCircleRadius
    //   
    //   _circlesRatioCaption
    //   _numberOfCirclesCaption
    //   _penPositionCaption
    //   _penWidthCaption
    //
    //   _batchSize
    //   _circles
    //   _currentIteration
    //   _indexOfLastCircle
    //   _isDrawing
    //   _numberOfIterations
    //   _penPoint
    // -------------------------------------------------------------------------

    #region .  Private Variables  .

    private const    int          _minNumberOfCircles = 2;
    private const    int          _maxNumberOfCircles = 4;
    private const    float        _minCircleRatio     = 0.1f;
    private const    float        _maxCircleRatio     = 0.9f;

    private readonly float        _angleIncrement     = 1.1f;
    private readonly int          _batchSizeMin       = 8;
    private readonly int          _batchSizeMax       = 30;
    private readonly float        _largeCircleRadius;

    private          int          _batchSize;
    private          GameObject[] _circles;
    private          int          _currentIteration;
    private          int          _indexOfLastCircle;
    private          bool         _isDrawing          = false;
    private          int          _numberOfIterations = 12000;
    private          GameObject   _penPoint;

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   OnCirclesRatioChanged()
    //   OnNumberOfCirclesValueChanged()
    //   OnPenPositionChanged()
    //   OnPenWidthChanged()
    //   Reset()
    //   RunDrawing()
    // -------------------------------------------------------------------------

    #region .  OnCirclesRatioChanged()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnCirclesRatioChanged()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void OnCirclesRatioChanged()
    {
        Debug.Log($"Spirograph.OnCirclesRatioChanged()");

        SmallCircleRadiusRatio  = _circlesRatioSlider.value;
        _circlesRatioValue.text = _circlesRatioSlider.value.ToString("0.00");

        for (int i = 1; i < NumberOfCircles; i++)
        {
            Circle circle = _circles[i].GetComponent<Circle>();
            circle.SetRadius(SmallCircleRadiusRatio);

            float centerY = 4 - 4 * Mathf.Pow(SmallCircleRadiusRatio, i);

            circle.SetCenter(new Vector3(0f, centerY, 0f) + transform.position);
            float sign = Mathf.Pow(-1f, i);

            _circles[i].GetComponent<Circle>().angleIncrement = sign * (_angleIncrement / Mathf.Pow(SmallCircleRadiusRatio, i));
        }

        OnPenPositionChanged();

    }   //  OnCirclesRatioChanged()
    #endregion


    #region .  OnNumberOfCirclesValueChanged()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnNumberOfCirclesValueChanged()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void OnNumberOfCirclesValueChanged()
    {
        NumberOfCircles = int.Parse(_numberOfCirclesDropdown.captionText.text);

        Debug.Log($"Spirograph.OnNumberOfCirclesValueChanged():  NumberOfCircles = {NumberOfCircles}");

        int index = _numberOfCirclesDropdown.value + 2;

        for (int i = 0; i < _circles.Length; i++)
        {
            _circles[i].SetActive(i < index);
        }

        _penPoint.transform.SetParent(_circles[index - 1].transform, false);
        _indexOfLastCircle = index - 1;

        OnPenPositionChanged();

    }   //  OnNumberOfCirclesValueChanged()
    #endregion


    #region .  OnPenPositionChanged()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnPenPositionChanged()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void OnPenPositionChanged()
    {
        _penPositionValue.text = _penPositionSlider.value.ToString("0.00");

        Debug.Log($"Spirograph.OnNumberOfCirclesValueChanged():  _penPositionValue = {_penPositionValue}");

        float penPointPosY     = (_penPositionSlider.value * Mathf.Pow(SmallCircleRadiusRatio, _indexOfLastCircle))
                               + _circles[_indexOfLastCircle].transform.position.y;

        _penPoint.GetComponent<PenPoint>().SetCenter(new Vector3(0f, penPointPosY, 0f) + transform.position);

    }   // OnPenPositionChanged()
    #endregion


    #region .  OnPenWidthChanged()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnPenWidthChanged()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void OnPenWidthChanged()
    {
        _penWidthValue.text = _penWidthSlider.value.ToString("0.00");

        Debug.Log($"Spirograph.OnPenWidthChanged():  _penWidthValue = {_penWidthValue}");

        _penPoint.GetComponent<PenPoint>().SetPenWidth(_penWidthSlider.value);

    }   //  OnPenWidthChanged()
    #endregion


    #region .  Reset()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Reset()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void Reset()
    {
        Debug.Log($"Spirograph.Reset()");

        _isDrawing = false;

        _penPoint.GetComponent<PenPoint>().ResetCurve();

        OnPenPositionChanged();
        OnCirclesRatioChanged();
        SetInteractable(true);
        ToggleSpritesVisible(true);

    }   // Reset()
    #endregion


    #region .  RunDrawing()  .
    // -------------------------------------------------------------------------
    //  Method.......:  RunDrawing()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void RunDrawing()
    {
        Debug.Log($"Spirograph.RunDrawing()");

        SetInteractable(false);

        _currentIteration = 0;
        _isDrawing        = true;

        _penPoint.GetComponent<PenPoint>().SetPositionArray(_numberOfIterations);

        StartCoroutine(Draw());
        SetInteractable(true);

    }   // RunDrawing()
    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Awake()
    //   Draw()
    //   OnDestroy()
    //   OnInputFieldTextChanged()
    //   SetInteractable()
    //   Start()
    //   ToggleSpritesVisible
    // -------------------------------------------------------------------------

    #region .  Awake()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Awake()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Awake()
    {
        _circlesRatioValue.text = _circlesRatioSlider.value.ToString("0.00");
        _penPositionValue .text = _penPositionSlider .value.ToString("0.00");
        _penWidthValue    .text = _penWidthSlider    .value.ToString("0.00");

        // Set the content type to Integer Number (no decimal).
        _inputIterations.contentType    = TMP_InputField.ContentType.IntegerNumber;
        _inputIterations.characterLimit = 5;
        _inputIterations.ForceLabelUpdate();

        // Subscribe to value change event.
        //_numberOfCirclesDropdown.onValueChanged.AddListener(OnNumberOfCirclesValueChanged);

        // Subscribe to end edit event.
        _inputIterations.onEndEdit.AddListener(OnInputFieldEndEdit);

    }   // Awake()
    #endregion


    #region .  Draw()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Draw()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private protected IEnumerator Draw()
    {
        while (_currentIteration < _numberOfIterations && _isDrawing)
        {
            int max = (_currentIteration + _batchSize < _numberOfIterations)
                    ?  _currentIteration + _batchSize
                    :  _numberOfIterations;

            for (int i = _currentIteration; i < max; i++)
            {
                for (int j = _circles.Length - 1; j >= 0; j--)
                {
                    Circle circle = _circles[j].GetComponent<Circle>();
                    circle.Iterate();
                }

                _penPoint.GetComponent<PenPoint>().StorePoint(i);
            }

            // Speed up drawing by increasing batchsize.
            _currentIteration += _batchSize;
            _currentIteration  = (_currentIteration < _numberOfIterations)
                               ?  _currentIteration
                               :  _numberOfIterations;
            
            _currentIterationValue.text = _currentIteration.ToString();

            _penPoint.GetComponent<PenPoint>().DrawCurve(_currentIteration);

            int batchSizeDelta = Mathf.RoundToInt(Mathf.Sqrt(_currentIteration * 0.1f));    // Mathf.RoundToInt(Mathf.Log10(10 + currentIteration));

            _batchSize = (_batchSizeMin + batchSizeDelta < _batchSizeMax)
                       ?  _batchSizeMin + batchSizeDelta
                       :  _batchSizeMax;

            yield return new WaitForSeconds(DelaySeconds);
        }

        ToggleSpritesVisible(!_isDrawing);

    }   //  Draw()
    #endregion


    #region .  OnDestroy()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnDestroy()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Always unsubscribe to prevent memory leaks.
        _inputIterations    .onValueChanged.RemoveListener(OnInputFieldEndEdit);

    }   //  OnDestroy()
    #endregion


    #region .  OnInputFieldEndEdit()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnInputFieldEndEdit()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnInputFieldEndEdit(string text)
    {
        Debug.Log($"Spirograph._tmp_InputIterations:  text changed to: {text}");

        _numberOfIterations = int.Parse(text);

    }   //  OnInputFieldEndEdit()
    #endregion


    #region .  SetInteractable()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetInteractable()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void SetInteractable(bool condition)
    {
        _drawButton             .interactable = condition;
        _circlesRatioSlider     .interactable = condition;
        _penPositionSlider      .interactable = condition;
        _numberOfCirclesDropdown.interactable = condition;

    }   //  SetInteractable()
    #endregion


    #region .  Start()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Start()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Start()
    {
        _numberOfCirclesDropdown.SetValueWithoutNotify(NumberOfCircles);

        SmallCircleRadiusRatio = _circlesRatioSlider.value;
        NumberOfCircles        = int.Parse(_numberOfCirclesDropdown.options[_numberOfCirclesDropdown.value].text);
        _numberOfIterations    = int.Parse(_inputIterations.text);

        // Construct Circle hierarchy
        _circles    = new GameObject[NumberOfCircles];
        _circles[0] = Instantiate(_circlePrefab);

        GameObject first = _circles[0];
        first.GetComponent<Circle>()        .center           = transform.position;
        first.GetComponent<Circle>()        .angleIncrement   = _angleIncrement;
        first.GetComponent<SpriteRenderer>().material         = _largeCircleMat;
        first.GetComponent<SpriteRenderer>().sprite           = _largeCircleSprite;
        first.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";

        _indexOfLastCircle = NumberOfCircles - 1;

        for (int i = 1; i < NumberOfCircles; i++)
        {
            _circles[i] = Instantiate(_circlePrefab);

            GameObject circle = _circles[i];
            circle.transform.SetParent(_circles[i - 1].transform, false);
            circle.GetComponent<SpriteRenderer>().material     = _smallCirclesMat;
            circle.GetComponent<SpriteRenderer>().sprite       = _smallCircleSprite;
            circle.GetComponent<SpriteRenderer>().sortingOrder = i;

            float sign = 1f - ((float)i % 2) * 2;       // Mathf.Pow(-1, i);
            circle.GetComponent<Circle>().angleIncrement = sign * (_angleIncrement / Mathf.Pow(SmallCircleRadiusRatio, (float)i));
        }

        for (int i = 1; i < NumberOfCircles; i++) 
        {
            Circle circle = _circles[i].GetComponent<Circle>();
            circle.SetRadius(SmallCircleRadiusRatio);

            float centerY = 4 - 4 * Mathf.Pow(SmallCircleRadiusRatio, (float)i);
            circle.SetCenter(new Vector3(0f, centerY, 0f) + transform.position);
        }

        _penPoint = Instantiate(_penPointPrefab);
        _penPoint.transform.SetParent(_circles[^1].transform, false);

        _numberOfCirclesDropdown.value = 1;
        _numberOfCirclesDropdown.RefreshShownValue();

        //OnNumberOfCirclesValueChanged(_numberOfCirclesDropdown);

    }   //  Start()
    #endregion


    #region .  ToggleSpritesVisible()  .
    // -------------------------------------------------------------------------
    //  Method.......:  ToggleSpritesVisible()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void ToggleSpritesVisible(bool condition)
    {
        for (int i = 1; i < _circles.Length; i++)
        {
            _circles[i].GetComponent<SpriteRenderer>().enabled = condition;
        }

        _penPoint.GetComponent<PenPoint>().SpriteVisible(condition);

    }   // ToggleSpritesVisible()
    #endregion


}   //class Spirograph
