using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Crosshair : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Static Events and Delegates:
    // -----------------------------------
    //   OnBrickCountChanged
    // -------------------------------------------------------------------------

    #region .  Public Events  .

    //public static event Action<int> OnBrickCountChanged = delegate { };

    #endregion



    // -------------------------------------------------------------------------
    // Public Variables:
    // -----------------
    //   Variable
    // -------------------------------------------------------------------------

    #region .  Public Variables  .

    public SpriteRenderer _spriteRenderer;
    public float SpinSpeed = 90f;

    #endregion



    // -------------------------------------------------------------------------
    // SerializeField Private Variables:
    // ---------------------------------
    //   _variable
    // -------------------------------------------------------------------------

    #region .  Private Variables  .

    //[SerializeField] private float _variable = 0f;

    #endregion



    // -------------------------------------------------------------------------
    // Private Variables:
    // ------------------
    //   _variable
    // -------------------------------------------------------------------------

    #region .  Private Variables  .

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   Method()
    // -------------------------------------------------------------------------

    #region .  Method()  .
    //// -------------------------------------------------------------------------
    ////   Method.......:  Method()
    ////   Description..:  
    ////   Parameters...:  None
    ////   Returns......:  Nothing
    //// -------------------------------------------------------------------------
    //public void Method()
    //{
    //}   // Method()
    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Start()
    //   Update()
    // -------------------------------------------------------------------------

    #region .  Start()  .
    //// -------------------------------------------------------------------------
    ////   Method.......:  Start()
    ////   Description..:  
    ////   Parameters...:  None
    ////   Returns......:  Nothing
    //// -------------------------------------------------------------------------
    //private void Start()
    //{
    //}   // Start()
    #endregion


    #region .  Update()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Update()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Update()
    {
        // Spin the wheel on its local axis
        transform.Rotate(Vector3.forward * SpinSpeed * Time.deltaTime, Space.Self);


    }   // Awake()
    #endregion


}	// class Crosshair
