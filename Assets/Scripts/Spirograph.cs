using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Spirograph : MonoBehaviour
{
    [Range(0.0f, 1.0f)]                   public float delaySeconds    = 0.001f;
    [Range(minNumCircles, maxNumCircles)] public int   numberOfCircles = 2;
    [Range(minRatio,      maxRatio)]      public float smallerCircleRadiusRatio;



    [SerializeField] private GameObject CirclePrefab;
    [SerializeField] private GameObject PenPointPrefab;
    [SerializeField] private Slider     CircleRatioSlider;
    [SerializeField] private Slider     PenPositionSlider;
    [SerializeField] private Slider     PenWidthSlider;
    [SerializeField] private Dropdown   NumCircDropdown;
    [SerializeField] private Button     drawButton;
    [SerializeField] private Material   largeCircleMat;
    [SerializeField] private Material   smallCirclesMat;
    [SerializeField] private Sprite     largeCircleSprite;
    [SerializeField] private Sprite     smallCircleSprite;

    private const int   minNumCircles = 2;
    private const int   maxNumCircles = 4;
    private const float minRatio      = 0.1f;
    private const float maxRatio      = 0.9f;


    private  GameObject[] Circles;
    private  GameObject   penPoint;

    private int   indexOfLastCircle;
    private float angleIncr      = 1.1f;
    private bool  drawing;
    private int   currentIteration;
    private float largeCircleRadius;
    private int   batchSize;
    private int   batchSizeMin   = 8;
    private int   batchSizeMax   = 30;
    private int   numIterations  = 12000;



    void Start()
    {
        smallerCircleRadiusRatio = CircleRatioSlider.value;

        // Construct Circle hierarchy
        Circles = new GameObject[numberOfCircles];

        Circles[0] = Instantiate(CirclePrefab);
        GameObject first = Circles[0];
        first.GetComponent<Circle>().center = transform.position;
        first.GetComponent<Circle>().angleIncrement = angleIncr;
        first.GetComponent<SpriteRenderer>().material = largeCircleMat;
        first.GetComponent<SpriteRenderer>().sprite = largeCircleSprite;
        first.GetComponent<SpriteRenderer>().sortingLayerName = "Sun";

        indexOfLastCircle = numberOfCircles - 1;

        for (int i = 1; i < numberOfCircles; i++)
        {
            Circles[i] = Instantiate(CirclePrefab);
            GameObject current = Circles[i];
            current.transform.SetParent(Circles[i - 1].transform, false);
            current.GetComponent<SpriteRenderer>().material = smallCirclesMat;
            current.GetComponent<SpriteRenderer>().sprite = smallCircleSprite;
            current.GetComponent<SpriteRenderer>().sortingOrder = i;
            float sign = 1 - (i % 2) * 2; // Mathf.Pow(-1, i);
            current.GetComponent<Circle>().angleIncrement = sign * (angleIncr / Mathf.Pow(smallerCircleRadiusRatio, i));
        }

        for (int i = 1; i < numberOfCircles; i++)
        {
            Circle circleComp = Circles[i].GetComponent<Circle>();
            circleComp.SetRadius(smallerCircleRadiusRatio);
            float centerY = 4 - 4*Mathf.Pow(smallerCircleRadiusRatio, i);
            circleComp.SetCenter(new Vector3(0, centerY, 0) + transform.position);
        }

        penPoint = Instantiate(PenPointPrefab);
        penPoint.transform.SetParent(Circles[Circles.Length - 1].transform, false);
        SetPenPointPosition();
        NumCircDropdown.value = 1;
        OnNumberOfCirclesValueChanged();

    }


    public void RunDrawing()
    {
        SetInteractable(false);
        currentIteration = 0;
        drawing = true;
        penPoint.GetComponent<PenPoint>().SetPositionArray(numIterations);
        StartCoroutine(Draw());

    }


    void SetInteractable(bool condition)
    {
        drawButton       .interactable = condition;
        CircleRatioSlider.interactable = condition;
        PenPositionSlider.interactable = condition;
        NumCircDropdown  .interactable = condition;

    }


    public void SetPenPointPosition()
    {
        PenPoint penComp = penPoint.GetComponent<PenPoint>();
        Circle lastCircleComp = Circles[indexOfLastCircle].GetComponent<Circle>();
        float penPointPosY = PenPositionSlider.value * Mathf.Pow(smallerCircleRadiusRatio, indexOfLastCircle) + Circles[indexOfLastCircle].transform.position.y;
        penComp.SetCenter(new Vector3(0, penPointPosY, 0) + transform.position);
    }

    public void SetPenWidth()
    {
    }


    public void Reset()
    {
        drawing = false;
        penPoint.GetComponent<PenPoint>().ResetCurve();
        SetPenPointPosition();
        ToggleSpritesVisible(true);
        OnSmallerCircleRatioChanged();
        SetInteractable(true);

    }


    protected IEnumerator Draw()
    {
        while (currentIteration < numIterations && drawing)
        {
            int max = (currentIteration + batchSize < numIterations ? currentIteration + batchSize : numIterations);

            for (int i = currentIteration; i < max; i++)
            {
                for (int j = Circles.Length - 1; j >= 0; j--)
                {
                    Circle circleComp = Circles[j].GetComponent<Circle>();
                    circleComp.Iterate();

                }
                penPoint.GetComponent<PenPoint>().StorePoint(i);
            }

            // Speed up drawing by increasing batchsize
            currentIteration += batchSize;
            currentIteration = (currentIteration < numIterations ? currentIteration : numIterations);
            penPoint.GetComponent<PenPoint>().DrawCurve(currentIteration);
            int batchSizeDelta = Mathf.RoundToInt(Mathf.Sqrt(currentIteration * 0.1f)); //Mathf.RoundToInt(Mathf.Log10(10 + currentIteration));
            batchSize = (batchSizeMin + batchSizeDelta < batchSizeMax ? batchSizeMin + batchSizeDelta : batchSizeMax);

            yield return new WaitForSeconds(delaySeconds);
        }

        ToggleSpritesVisible(!drawing);

    }


    void ToggleSpritesVisible(bool condition)
    {
        for (int i = 1; i < Circles.Length; i++)
        {
            Circles[i].GetComponent<SpriteRenderer>().enabled = condition;
        }

        penPoint.GetComponent<PenPoint>().SpriteVisible(condition);

    }


    public void OnNumberOfCirclesValueChanged()
    {
        int index = NumCircDropdown.value + 2;

        for (int i = 0; i < Circles.Length; i++)
        {
            Circles[i].SetActive(i < index);
        }

        penPoint.transform.SetParent(Circles[index - 1].transform, false);
        indexOfLastCircle = index - 1;
        SetPenPointPosition();

    }


    public void OnSmallerCircleRatioChanged()
    {
        smallerCircleRadiusRatio = CircleRatioSlider.value;

        for (int i = 1; i < numberOfCircles; i++)
        {
            Circle circleComp = Circles[i].GetComponent<Circle>();
            circleComp.SetRadius(smallerCircleRadiusRatio);
            float centerY = 4 - 4 * Mathf.Pow(smallerCircleRadiusRatio, i);
            circleComp.SetCenter(new Vector3(0, centerY, 0) + transform.position);
            float sign = Mathf.Pow(-1, i);
            Circles[i].GetComponent<Circle>().angleIncrement = sign * (angleIncr / Mathf.Pow(smallerCircleRadiusRatio, i));
        }

        SetPenPointPosition();

    }


}
