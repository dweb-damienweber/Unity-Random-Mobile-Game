using UnityEngine;

public class ReturnZone : MonoBehaviour
{
    #region Events

    public delegate void SimpleEvent();
    public event SimpleEvent BallReturnEvent;

    #endregion

    
    #region System methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ObjectPool.AddToPool(other.gameObject);

            if (BallReturnEvent != null)
                BallReturnEvent.Invoke();
        }
    }

    #endregion
}
