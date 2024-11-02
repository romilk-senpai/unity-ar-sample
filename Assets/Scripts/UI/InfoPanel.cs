using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI geospatialInfoText;
    [SerializeField] private TextMeshProUGUI locationInfoText;

    public void SetGeospatialMessage(string message)
    {
        geospatialInfoText.text = message;
    }

    public void SetLocationInfoText(string message)
    {
        locationInfoText.text = message;
    }
}
