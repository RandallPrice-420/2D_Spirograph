using System.Collections;
using TMPro; // Required for TMP_InputField
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
    [Range(0, 20000)]   public int   NumberOfIterations = 2;

    [Range(_minNumberOfCircles, _maxNumberOfCircles)] public int   NumberOfCircles = 2;
    [Range(_minCircleRatio,     _maxCircleRatio)]     public float SmallerCircleRadiusRatio;

    #endregion



    // -------------------------------------------------------------------------
    // Private Serialize Variables:
    // ----------------------------
    //   _circlePrefab
    //   _penPointPrefab
    //   _circlesRatioSlider
    //   _penPositionSlider
    //   _penWidthSlider
    //   _numberOfCirclesDropdown
    //   _drawButton
    //   _largeCircleMat
    //   _smallCirclesMat
    //   _largeCircleSprite
    //   _smallCircleSprite
    //   _tmp_InputIterations
    // -------------------------------------------------------------------------

    #region .  Private Serialize Variables  .

    [SerializeField] private GameObject     _circlePrefab;
    [SerializeField] private GameObject     _penPointPrefab;
    [SerializeField] private Slider         _circlesRatioSlider;
    [SerializeField] private Slider         _penPositionSlider;
    [SerializeField] private Slider         _penWidthSlider;
    [SerializeField] private Dropdown       _numberOfCirclesDropdown;
    [SerializeField] private Button         _drawButton;
    [SerializeField] private Material       _largeCircleMat;
    [SerializeField] private Material       _smallCirclesMat;
    [SerializeField] private Sprite         _largeCircleSprite;
    [SerializeField] private Sprite         _smallCircleSprite;
    [SerializeField] private TMP_InputField _tmp_InputIterations;

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
        // Validate reference
        if (_tmp_InputIterations == null)
        {
            Debug.LogError("TMP_InputField reference is missing!");
            return;
        }

        // Subscribe to the TMP_InputField onValueChanged event
        _tmp_InputIterations.onValueChanged.AddListener(OnInputFieldTextChanged);

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
            int max = (_currentIteration + _batchSize < _numberOfIterations ? _currentIteration + _batchSize : _numberOfIterations);

            for (int i = _currentIteration; i < max; i++)
            {
                for (int j = _circles.Length - 1; j >= 0; j--)
                {
                    Circle circleComp = _circles[j].GetComponent<Circle>();
                    circleComp.Iterate();
                }

                _penPoint.GetComponent<PenPoint>().StorePoint(i);
            }

            // Speed up drawing by increasing batchsize
            _currentIteration += _batchSize;
            _currentIteration = (_currentIteration < _numberOfIterations ? _currentIteration : _numberOfIterations);

            _penPoint.GetComponent<PenPoint>().DrawCurve(_currentIteration);

            int batchSizeDelta = Mathf.RoundToInt(Mathf.Sqrt(_currentIteration * 0.1f)); //Mathf.RoundToInt(Mathf.Log10(10 + currentIteration));
            _batchSize = (_batchSizeMin + batchSizeDelta < _batchSizeMax ? _batchSizeMin + batchSizeDelta : _batchSizeMax);

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
        if (_tmp_InputIterations != null)
        {
            _tmp_InputIterations.onValueChanged.RemoveListener(OnInputFieldTextChanged);
        }

    }   //  OnDestroy()
    #endregion


    #region .  OnInputFieldTextChanged()  .
    // -------------------------------------------------------------------------
    //  Method.......:  OnInputFieldTextChanged()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnInputFieldTextChanged(string newText)
    {
        Debug.Log($"Text changed to: {newText}");

        // Example: Limit input length
        if (newText.Length > 10)
        {
            _tmp_InputIterations.text = newText[..10];
            Debug.LogWarning("Input truncated to 10 characters.");
        }

        _numberOfIterations = int.Parse(_tmp_InputIterations.text);

    }   //  OnInputFieldTextChanged()
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
        _numberOfIterations      = int.Parse(_tmp_InputIterations.text);
        SmallerCircleRadiusRatio = _circlesRatioSlider.value;

        // Construct Circle hierarchy
        _circles = new GameObject[NumberOfCircles];

        _circles[0] = Instantiate(_circlePrefab);

        GameObject first                                      = _circles[0];
        first.GetComponent<Circle>()        .center           = transform.position;
        first.GetComponent<Circle>()        .angleIncrement   = _angleIncrement;
        first.GetComponent<SpriteRenderer>().material         = _largeCircleMat;
        first.GetComponent<SpriteRenderer>().sprite           = _largeCircleSprite;
        first.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";

        _indexOfLastCircle = NumberOfCircles - 1;

        for (int i = 1; i < NumberOfCircles; i++)
        {
            _circles[i] = Instantiate(_circlePrefab);

            GameObject current                                  = _circles[i];
            current.transform.SetParent(_circles[i - 1].transform, false);
            current.GetComponent<SpriteRenderer>().material     = _smallCirclesMat;
            current.GetComponent<SpriteRenderer>().sprite       = _smallCircleSprite;
            current.GetComponent<SpriteRenderer>().sortingOrder = i;

            float sign = 1 - (i % 2) * 2;       // Mathf.Pow(-1, i);
            current.GetComponent<Circle>().angleIncrement = sign * (_angleIncrement / Mathf.Pow(SmallerCircleRadiusRatio, i));
        }

        for (int i = 1; i < NumberOfCircles; i++)
        {
            Circle circleComp = _circles[i].GetComponent<Circle>();
            circleComp.SetRadius(SmallerCircleRadiusRatio);

            float centerY = 4 - 4 * Mathf.Pow(SmallerCircleRadiusRatio, i);
            circleComp.SetCenter(new Vector3(0f, centerY, 0f) + transform.position);
        }

        _penPoint = Instantiate(_penPointPrefab);
        _penPoint.transform.SetParent(_circles[^1].transform, false);

        SetPenPointPosition();

        _numberOfCirclesDropdown.value = 1;
        OnNumberOfCirclesValueChanged();

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



    public void OnNumberOfCirclesValueChanged()
    {
        int index = _numberOfCirclesDropdown.value + 2;
        index = 2;

        for (int i = 0; i < _circles.Length; i++)
        {
            _circles[i].SetActive(i < index);
        }

        _penPoint.transform.SetParent(_circles[index - 1].transform, false);
        _indexOfLastCircle = index - 1;

        SetPenPointPosition();

    }   //  OnNumberOfCirclesValueChanged()


    public void OnSmallerCircleRatioChanged()
    {
        SmallerCircleRadiusRatio = _circlesRatioSlider.value;

        for (int i = 1; i < NumberOfCircles; i++)
        {
            Circle circleComp = _circles[i].GetComponent<Circle>();
            circleComp.SetRadius(SmallerCircleRadiusRatio);

            float centerY = 4 - 4 * Mathf.Pow(SmallerCircleRadiusRatio, i);

            circleComp.SetCenter(new Vector3(0f, centerY, 0f) + transform.position);
            float sign = Mathf.Pow(-1f, i);

            _circles[i].GetComponent<Circle>().angleIncrement = sign * (_angleIncrement / Mathf.Pow(SmallerCircleRadiusRatio, i));
        }

        SetPenPointPosition();

    }   //  OnSmallerCircleRatioChanged()


    public void Reset()
    {
        _isDrawing = false;
        _penPoint.GetComponent<PenPoint>().ResetCurve();

        SetPenPointPosition();
        ToggleSpritesVisible(true);
        OnSmallerCircleRatioChanged();
        SetInteractable(true);

    }   //  Reset()


    public void RunDrawing()
    {
        SetInteractable(false);

        _currentIteration = 0;
        _isDrawing        = true;

        _penPoint.GetComponent<PenPoint>().SetPositionArray(_numberOfIterations);

        StartCoroutine(Draw());
        SetInteractable(true);

    }   //  RunDrawing()


    public void SetPenPointPosition()
    {
        PenPoint penComp        = _penPoint.GetComponent<PenPoint>();
        Circle   lastCircleComp = _circles[_indexOfLastCircle].GetComponent<Circle>();

        float    penPointPosY   = (_penPositionSlider.value * Mathf.Pow(SmallerCircleRadiusRatio, _indexOfLastCircle))
                                + _circles[_indexOfLastCircle].transform.position.y;

        penComp.SetCenter(new Vector3(0f, penPointPosY, 0f) + transform.position);

    }   //  SetPenPointPosition()


    public void SetPenWidth()
    {
    }   //  SetPenWidth()


}   //class Spirograph
