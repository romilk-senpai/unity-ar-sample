using UnityEngine;
using Zenject;

public class SampleInputController : MonoBehaviour
{
    [Inject] private IInputHandler _inputHandler;

    // very simple input controller with only one feature communicating with anstract Input Handler
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inputHandler.ProcessClick(Input.mousePosition);
        }
    }
}
