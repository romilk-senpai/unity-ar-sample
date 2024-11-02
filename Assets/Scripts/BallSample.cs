using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BallSample : MonoBehaviour, IInputHandler
{
    [Space(10)]
    [SerializeField] private Ball ballPrefab;
    // after this time ball with disable and go to pool
    [SerializeField] private float ballLifetime = 5f;
    [SerializeField] private float throwForce = 500f;
    // layermask which will filter objects we want to apply vibration on collision
    [SerializeField] private LayerMask vibrationTriggerLayerMask;

    private Camera _mainCamera;

    // apply a little bit of pulling
    private Queue<Ball> _ballPool;

    private void Start()
    {
        // maybe it is better to inject it with zenject, i'm not too sure
        _mainCamera = Camera.main;
        _ballPool = new Queue<Ball>();
    }

    private void OnBallCollision(Collision collision)
    {
        // check if object is in our trigger layer
        if ((vibrationTriggerLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            // this seems pretty basic but fine for our task
            Handheld.Vibrate();
        }
    }

    // handling wait with UniTask
    public async void ProcessClick(Vector2 clickPosition)
    {
        // so we spawn ball where user pressed on the screen
        Vector3 spawnPos = _mainCamera.ScreenToWorldPoint(clickPosition);

        Ball ball;

        if (!_ballPool.TryDequeue(out ball))
        {
            ball = Instantiate(ballPrefab);
            ball.OnBallCollision += OnBallCollision;
        }

        // re-enabling pulled objects
        ball.gameObject.SetActive(true);
        ball.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        // applying force towards camera forward direction
        ball.ApplyForce(_mainCamera.transform.forward * throwForce);

        // it throws exception on cancel and i'm pretty sure it doesn't matter
        // but it's chained by input system therefore it's better to handle the exception
        bool isCanceled = await UniTask.WaitForSeconds(ballLifetime, cancellationToken: destroyCancellationToken)
            .SuppressCancellationThrow();

        if (isCanceled)
        {
            return;
        }

        ball.gameObject.SetActive(false);
        _ballPool.Enqueue(ball);
    }
}
