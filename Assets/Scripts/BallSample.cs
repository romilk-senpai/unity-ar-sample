using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSample : MonoBehaviour
{
    // simple reference to click action
    [SerializeField] private InputActionProperty clickAction;

    [Space(10)]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private float ballLifetime = 5f;
    [SerializeField] private float throwForce = 500f;
    // layermask which will filter objects we want to apply vibration on collision
    [SerializeField] private LayerMask vibrationTriggerLayerMask;

    private Camera _mainCamera;

    // apply a little bit of pulling
    private Queue<Ball> _ballPool;

    private void Start()
    {
        _mainCamera = Camera.main;
        _ballPool = new Queue<Ball>();
        clickAction.action.performed += OnClickPerformed;
        clickAction.action.Enable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        var touchPosition = context.ReadValue<Vector2>();

        _mainCamera.ScreenToWorldPoint(touchPosition);

        if (!_ballPool.TryDequeue(out Ball ball))
        {
            ball = Instantiate(ballPrefab);
            ball.OnBallCollision += OnBallCollision;
        }

        ball.gameObject.SetActive(true);
        ball.transform.SetPositionAndRotation(_mainCamera.transform.position, Quaternion.identity);
        ball.ApplyForce(_mainCamera.transform.forward * throwForce);

        StartCoroutine(DisableBallIn(ball, ballLifetime));
    }

    private IEnumerator DisableBallIn(Ball ball, float timer)
    {
        yield return new WaitForSeconds(timer);
        ball.gameObject.SetActive(false);
        _ballPool.Enqueue(ball);
    }

    private void OnBallCollision(Collision collision)
    {
        // check if object is in our trigger layer
        if ((vibrationTriggerLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            Handheld.Vibrate();
        }
    }
}
