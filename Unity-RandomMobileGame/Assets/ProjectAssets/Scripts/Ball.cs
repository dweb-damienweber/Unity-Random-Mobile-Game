using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    #region Properties

    public Transform Transform
    {
        get;
        set;
    }

    public Rigidbody2D RigidBody
    {
        get;
        private set;
    }

    #endregion


    #region System methods

    private void Awake()
    {
        if (Transform == null)
        {
            Transform = GetComponent<Transform>();
            RigidBody = GetComponent<Rigidbody2D>();
        }
    }

    #endregion
}
