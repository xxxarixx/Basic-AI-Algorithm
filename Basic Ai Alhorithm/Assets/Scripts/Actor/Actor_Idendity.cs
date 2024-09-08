using CropField;
using System;
using UnityEngine;

namespace Actor
{
    [DefaultExecutionOrder(-10)]
    public class Actor_Idendity : MonoBehaviour
    {
        public CropHole actorAssignToCropHole;
        public string actorID { get; private set; }
        private void Awake()
        {
            actorID = generateID();
        }
        private string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }
        public bool AssignFarmerToHole(ref CropHole cropHole)
        {
            if (cropHole.thereIsActorAssignedToThisHole)
                return false;
            actorAssignToCropHole = cropHole;
            actorAssignToCropHole.actorIdendity_AssignToThisHole = this;
            return true;
        }
        public void RemoveAssignFarmerToHole()
        {
            if (actorAssignToCropHole == null)
                return;
            if(!actorID.Equals(actorAssignToCropHole.actorIdendity_AssignToThisHole.actorID))
            {
                actorAssignToCropHole.actorIdendity_AssignToThisHole = null;
                return;
            }
            actorAssignToCropHole.actorIdendity_AssignToThisHole = null;
            actorAssignToCropHole = null;
        }
    }
}
