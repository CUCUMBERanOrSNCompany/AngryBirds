using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the slingshot logic.
/// </summary>
[RequireComponent(typeof(SFXManager))]
public class Slingshot : MonoBehaviour
{
    #region fields

    /// <summary>
    /// Array of line renderers
    /// </summary>
    public LineRenderer[] LineRenderers;

    /// <summary>
    /// The Sling's strips positions
    /// </summary>
    public Transform[] StripPositions;

    /// <summary>
    /// The center position of the sling
    /// </summary>
    public Transform Center;

    /// <summary>
    /// Idle position of the strips
    /// </summary>
    public Transform IdlePosition;

    /// <summary>
    /// The current position of the strips
    /// </summary>
    public Vector3 CurrentPosition;

    /// <summary>
    /// The maximum length of the sling's strips
    /// </summary>
    public float MaxLengthOfSling;

    /// <summary>
    /// Helping us in ensuring the strips aren't going through the ground
    /// </summary>
    public float BottomBoundary;

    /// <summary>
    /// Indicating if the mouse is clicked
    /// </summary>
    private bool _isMouseDown;

    /// <summary>
    /// Prefab of the bird
    /// </summary>
    public GameObject BirdPrefab;

    /// <summary>
    /// The offset of the bird to the sling.
    /// </summary>
    public float BirdPositionOffset;

    /// <summary>
    /// Rigidbody component of the bird.
    /// </summary>
    private Rigidbody2D _bird;

    /// <summary>
    /// Collider component of the bird
    /// </summary>
    private Collider2D _birdCollider;

    /// <summary>
    /// Current force applied.
    /// </summary>
    public float Force;

    /// <summary>
    /// Reference to the SFX Manager component
    /// </summary>
    private SFXManager _sfxManager = null;

    [SerializeField] private Trajectory _trajectory = null;

    #endregion

    #region Initializers
    /// <summary>
    /// Initialization Tasks.
    /// </summary>
    void Start()
    {
        _sfxManager = GetComponent<SFXManager>();

        InitializeStripLinesRenderers();

        CreateBird();
    }

    /// <summary>
    /// Initializing the sling's strip lines
    /// </summary>
    private void InitializeStripLinesRenderers()
    {
        LineRenderers[0].positionCount = 2;
        LineRenderers[1].positionCount = 2;
        LineRenderers[0].SetPosition(0, StripPositions[0].position);
        LineRenderers[1].SetPosition(0, StripPositions[1].position);
    }

    /// <summary>
    /// Creating the bird object
    /// </summary>
    void CreateBird()
    {
        _bird = Instantiate(BirdPrefab).GetComponent<Rigidbody2D>();
        _birdCollider = _bird.GetComponent<Collider2D>();
        _birdCollider.enabled = false;

        _bird.isKinematic = true;

        ResetStrips();
    }
    #endregion

    /// <summary>
    /// The monobehavior logic.
    /// </summary>
    void Update()
    {
        if (_isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            CurrentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            CurrentPosition = Center.position
                + Vector3.ClampMagnitude(CurrentPosition - Center.position,
                MaxLengthOfSling);

            CurrentPosition = ClampBoundary(CurrentPosition);

            SetStrips(CurrentPosition);

            if (_birdCollider)
            {
                _birdCollider.enabled = true;
                //AimingLine.Instance.UpdateAimingLine(CurrentPosition, Center, _bird);
                Vector3 birdForce = (CurrentPosition - Center.position) * Force * -1;
                _sfxManager.PlaySFXNoOverride(SoundsEnum.Aim);
            }
        }
        else
        {
            ResetStrips();
            //AimingLine.Instance.ShowHideAimingLine(false);
            //_trajectory.Hide();
        }
    }

    /// <summary>
    /// Defining behavior when the mouse is clicked
    /// </summary>
    private void OnMouseDown()
    {
        _isMouseDown = true;

        //AimingLine.Instance.ShowHideAimingLine(true);
        _trajectory.Show();

    }

    /// <summary>
    /// Defining behavior when the mouse is unclicked
    /// </summary>
    private void OnMouseUp()
    {
        _isMouseDown = false;
        Shoot();

        //AimingLine.Instance.UpdateAimingLine(CurrentPosition, Center, _bird);
        CurrentPosition = IdlePosition.position;
        //AimingLine.Instance.ShowHideAimingLine(false);
        _trajectory.Hide();
    }

    /// <summary>
    /// The shooting logic
    /// </summary>
    void Shoot()
    {
        if (_bird)
        {
            _bird.isKinematic = false;
            Vector3 birdForce = (CurrentPosition - Center.position) * Force * -1;
            _bird.velocity = birdForce;

            _sfxManager.PlaySFXOverride(SoundsEnum.Shoot);

            _bird.GetComponent<Bird>().Release();

            _bird = null;
            _birdCollider = null;
            Invoke("CreateBird", 2);
        }
    }

    /// <summary>
    /// Resetting the sling's strips position to idle.
    /// </summary>
    void ResetStrips()
    {
        CurrentPosition = IdlePosition.position;
        SetStrips(CurrentPosition);
    }

    /// <summary>
    /// Changing the sling's strip position
    /// </summary>
    /// <param name="position">The new position of the strips</param>
    void SetStrips(Vector3 position)
    {
        LineRenderers[0].SetPosition(1, position);
        LineRenderers[1].SetPosition(1, position);

        if (_bird)
        {
            Vector3 dir = position - Center.position;
            _bird.transform.position = position + dir.normalized
                * BirdPositionOffset;

            // Calculate the angle of the slingshot
            float slingshotAngle = Mathf.Atan2(dir.y, dir.x);

            // Set the launch direction for the bird based on the slingshot angle
            _bird.transform.right = Quaternion.Euler(0, 0,
                Mathf.Rad2Deg * slingshotAngle) * Vector3.right;
        }
    }

    /// <summary>
    /// The boundary of the clamp
    /// </summary>
    /// <param name="vector">Unprocessed boundary vector</param>
    /// <returns>The input vector with y element clamped</returns>
    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, BottomBoundary, 1000);
        return vector;
    }

    
}
