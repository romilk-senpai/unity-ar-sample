using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private Collider ballCollider;

    public event Action<Collision> OnBallCollision;

    private void OnEnable()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        ballRigidbody.AddForce(force);
    }

    public void OnCollisionEnter(Collision col)
    {
        OnBallCollision?.Invoke(col);
    }
}
