using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundLoader : MonoBehaviour
{
    public Transform eastCourtSide;
    public Transform westCourtSide;
    public Transform southCourtSide;
    public Transform northCourtSide;

    void Start()
    {
        ScoreManager.GetInstance().loadReferee(eastCourtSide.position, westCourtSide.position,
                                            southCourtSide.position, northCourtSide.position);
        Debug.Log("creating referee");
    }
    
}
