using UnityEngine;


public class PenPoint : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   Center
    //   PenWidth
    //   MaterialColorStart
    //   MaterialColorEnd
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    public Vector3  Center;
    public float    PenWidth;
    public Material MaterialColorStart;
    public Material MaterialColorEnd;

    #endregion



    // -------------------------------------------------------------------------
    // Private Variables:
    // ------------------
    //   _lineRenderer
    //   _positionArray
    //   _spriteRenderer
    // -------------------------------------------------------------------------

    #region .  Private Variables  .

    private LineRenderer   _lineRenderer;
    private Vector3[]      _positionArray;
    private SpriteRenderer _spriteRenderer;

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   DrawCurve()
    //   ResetCurve()
    //   SetCenter()
    //   SetColor()
    //   SetPenWidth()
    //   SetPositionArray()
    //   SpriteVisible()
    //   StorePoint()
    // -------------------------------------------------------------------------

    #region .  DrawCurve()  .
    // -------------------------------------------------------------------------
    //  Method.......:  DrawCurve()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void DrawCurve(int count)
    {
        _lineRenderer.positionCount = count;
        _lineRenderer.SetPositions(_positionArray);

    }   // DrawCurve()
    #endregion


    #region .  ResetCurve()  .
    // -------------------------------------------------------------------------
    //  Method.......:  ResetCurve()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void ResetCurve()
    {
        _positionArray = new Vector3[0];
        DrawCurve(_positionArray.Length);

    }   // ResetCurve()
    #endregion


    #region .  SetCenter()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetCenter()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SetCenter(Vector3 center)
    {
        this.Center        = center;
        transform.position = center;

    }   // SetCenter()
    #endregion


    #region .  SetPenColor()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetPenColor()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SetPenColor(Material material, Color colorStart, Color colorEnd)
    {
        // Set material and color for visibility.
        _lineRenderer.material   = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = colorStart;
        _lineRenderer.endColor   = colorEnd;

    }   // SetPenColor()
    #endregion


    #region .  SetPenWidth()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetPenWidth()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SetPenWidth(float width)
    {
        // Set width (replaces SetWidth).
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth   = width;

    }   // SetPenWidth()
    #endregion


    #region .  SetPositionArray()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetPositionArray()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SetPositionArray(int size)
    {
        _positionArray = new Vector3[size];

    }   // SetPositionArray()
    #endregion


    #region .  SpriteVisible()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SpriteVisible()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SpriteVisible(bool condition)
    {
        _spriteRenderer.enabled = condition;

    }   // SpriteVisible()
    #endregion


    #region .  StorePoint()  .
    // -------------------------------------------------------------------------
    //  Method.......:  StorePoint()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void StorePoint(int index)
    {
        Vector3 newPosition   = transform.position;
        _positionArray[index] = newPosition;

    }   // StorePoint()
    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Start()
    // -------------------------------------------------------------------------

    #region .  Start()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Start()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lineRenderer   = GetComponent<LineRenderer>();

        _lineRenderer.startColor = MaterialColorStart.color;
        _lineRenderer.endColor   = MaterialColorEnd  .color;

    }   // Start()
    #endregion


}   // class PenPoint
