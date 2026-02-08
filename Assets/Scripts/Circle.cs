using UnityEngine;


public class Circle : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   angleIncrement
    //   center
    //   radius
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    public float   angleIncrement;
    public Vector3 center;
    public float   radius = 1f;

    #endregion



    // -------------------------------------------------------------------------
    // Private Variables:
    // ------------------
    //   _displaySprite
    //   _spriteRenderer
    // -------------------------------------------------------------------------

    #region .  Private Variables  .

    private SpriteRenderer _spriteRenderer;

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   Iterate()
    //   SetCenter()
    //   SetRadius()
    //   SpriteVisible()
    // -------------------------------------------------------------------------

    #region .  Iterate()  .
    // -------------------------------------------------------------------------
    //  Method.......:  Iterate()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void Iterate()
    {
        transform.Rotate(0f, 0f, angleIncrement);

    }   // Iterate()
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
        this.center        = center;
        transform.position = center;

    }   //  SetCenter()
    #endregion


    #region .  SetRadius()  .
    // -------------------------------------------------------------------------
    //  Method.......:  SetRadius()
    //  Description..:  
    //  Parameters...:  None
    //  Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void SetRadius(float radius)
    {
        Debug.Log($"Circle.SetRdius():  radius = {radius}");

        if (radius == 0)
        {
            radius = 1f;
        }

        this.radius          = radius;
        transform.localScale = new Vector3(radius, radius, 1f);

        Debug.Log($"Circle.SetRdius():  (transform.localScale = {transform.localScale}");

        if ((transform.localScale.x == 0) || (transform.localScale.y == 0))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }   //  SetRadius()
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

    }   //  SpriteVisible()
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
        _spriteRenderer    = GetComponent<SpriteRenderer>();
        transform.position = center;

        SetRadius(radius);

    }   //  Start()
    #endregion


}   // class Circle
