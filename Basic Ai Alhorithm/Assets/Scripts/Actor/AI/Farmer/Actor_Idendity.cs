using AI.Farmer;
using CropField;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Idendity : MonoBehaviour
{
    private CropHole actorAssignToCropHole;
    public string ID { get; private set; }
    private void Awake()
    {
        ID = generateID();
    }
    private string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }
    public void AssignFarmerToHole(CropHole cropHole)
    {
        actorAssignToCropHole = cropHole;
        cropHole.actorIdendity_AssignToThisHole = this;
    }
    public void RemoveAssignFarmerToHole()
    {
        if (actorAssignToCropHole == null)
            return;
        actorAssignToCropHole.actorIdendity_AssignToThisHole = null;
        actorAssignToCropHole = null;
    }
}
