using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class Cube : MonoBehaviour
{
    #region Event

    public delegate void CubeEvent(Cube cube);
    public delegate void ScoreEvent();

    public event CubeEvent CubeDeathEvent;
    public event ScoreEvent ScoreGainedEvent;

    #endregion


    #region Properties

    public Transform Transform
    {
        get;
        set;
    }

    #endregion

    
    #region System methods

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            Transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            InitCube(_health);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        TakeDamage();
    }

    #endregion


    #region Methods

    public void InitCube(int value)
    {
        _health = value;

        ChangeColor();
        UpdateCubeUI();
    }

    private void UpdateCubeUI()
    {
        _text.text = _health.ToString();
    }

    private void ChangeColor()
    {
        int randomIndex = Random.Range(0, _colors.Count);

        _spriteRenderer.color = _colors[randomIndex];
    }

    private void TakeDamage()
    {
        _health--;

        ChangeColorOnHit();
        UpdateCubeUI();

        if (ScoreGainedEvent != null)
            ScoreGainedEvent.Invoke();

        if (_health == 0)
        {
            if (CubeDeathEvent != null)
            {
                CubeDeathEvent.Invoke(this);

                ScoreGainedEvent = null;
                CubeDeathEvent = null;
            }

            ObjectPool.AddToPool(gameObject);
        }
        else
        {
            Invoke("ResetColorAfterHit", .1f);
        }
    }

    private void ChangeColorOnHit()
    {
        if (_spriteRenderer.color == Color.white)
            return;

        _previousColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.white;
    }

    private void ResetColorAfterHit()
    {
        _spriteRenderer.color = _previousColor;
    }

    #endregion


    #region Private fields

    [SerializeField] private int _health = 0;

    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private List<Color> _colors = new List<Color>();
    private SpriteRenderer _spriteRenderer = null;

    private Color _previousColor;

    #endregion
}
