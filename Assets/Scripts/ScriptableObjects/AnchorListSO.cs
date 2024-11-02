using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnchorList", menuName = "ScriptableObjects/AnchorList", order = 1)]
public class AnchorListSO : ScriptableObject
{
    [Header("Only Lat/Lon are required, rest are optional")]
    [SerializeField] private GeospatialAnchorHistoryCollection anchors;
    
    // provided through IReadOnlyList so user can't break anything inside
    // considering these are structs makes it even more safe :]
    // linq doesn't work with these as far as I know :c
    public IReadOnlyList<GeospatialAnchorHistory> Anchors => anchors.Collection;
}
