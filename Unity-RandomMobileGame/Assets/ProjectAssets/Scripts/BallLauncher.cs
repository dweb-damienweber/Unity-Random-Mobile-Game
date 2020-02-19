using System.Collections;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    #region Events

    public delegate void SimpleEvent();
    public event SimpleEvent NewPlayerTurnEvent;

    #endregion

    
    #region System methods

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (_canLaunch)
        {
            _mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonDown("Fire1"))
                StartDrag();
            else if (Input.GetButton("Fire1"))
                ContinueDrag();
            else if (Input.GetButtonUp("Fire1"))
                EndDrag();
        }
    }

    #endregion


    #region Methods

    private void Init()
    {
        _transform = GetComponent<Transform>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (_camera == null)
            Debug.LogError("No camera in scene", this);

        ReturnZone returnZone = FindObjectOfType<ReturnZone>();
        if (returnZone != null)
            returnZone.BallReturnEvent += OnBallReturn;
    }

    private void StartDrag()
    {
        _dragStart = _mouseWorldPosition;
    }

    private void ContinueDrag()
    {
        _dragEnd = _mouseWorldPosition;
    }

    private void EndDrag()
    {
        Vector2 direction = _dragEnd - _dragStart;
        direction.Normalize();

        if (direction.magnitude > 0)
        {
            _canLaunch = false;
            StartCoroutine(LaunchBalls(direction));
        }
    }

    private void OnBallReturn()
    {
        _ballReturned++;

        if (_ballReturned == _ballNumber)
        {
            _ballReturned = 0;

            if (NewPlayerTurnEvent != null)
                NewPlayerTurnEvent.Invoke();

            RandomLaunchPosition();
            _canLaunch = true;
        }
    }

    private void RandomLaunchPosition()
    {
        _transform.position = new Vector2(Random.Range(_minPositionX, _maxPositionX), _transform.position.y);
    }

    private IEnumerator LaunchBalls(Vector2 direction)
    {
        for (int i = 0; i < _ballNumber; i++)
        {
            Ball ball = ObjectPool.GetFromPool(_ballPrefab).GetComponent<Ball>();

            ball.Transform.position = _transform.position;
            ball.RigidBody.AddForce(-direction * 800f);

            yield return new WaitForSeconds(.1f);
        }
    }

    #endregion


    #region Private fields

    private Transform _transform = null;
    private Camera _camera = null;

    private Vector2 _mouseWorldPosition = Vector2.zero;

    private Vector2 _dragStart = Vector2.zero;
    private Vector2 _dragEnd = Vector2.zero;

    private bool _canLaunch = true;
    private int _ballNumber = 5;
    private int _ballReturned = 0;

    [SerializeField] private GameObject _ballPrefab = null;

    [SerializeField] private float _minPositionX = 0f;
    [SerializeField] private float _maxPositionX = 0f;

    #endregion
}
