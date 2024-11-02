using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnchorList", menuName = "ScriptableObjects/AnchorList", order = 1)]
public class AnchorListSO : ScriptableObject
{
    [SerializeField] private GeospatialAnchorHistoryCollection anchors;
    
    public IReadOnlyList<GeospatialAnchorHistory> Anchors => anchors.Collection;
}
