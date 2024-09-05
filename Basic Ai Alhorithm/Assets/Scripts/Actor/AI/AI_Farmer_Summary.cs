using AI.Farmer;
using System.Collections.Generic;
using UnityEngine;
namespace AI.Farmer
{
    [System.Serializable]
    public class AI_Farmer_Summary
    {
        private List<AI_Farmer_Dependencies> ai_farmers = new List<AI_Farmer_Dependencies>();
        public List<Actor_Idendity> farmers_inSeedJob = new List<Actor_Idendity>();
        public List<Actor_Idendity> farmers_inCropJob = new List<Actor_Idendity>();
        public void SubscribeToFarmersStateChange()
        {
            ai_farmers.Clear();
            ai_farmers.AddRange(Object.FindObjectsByType<AI_Farmer_Dependencies>(findObjectsInactive:FindObjectsInactive.Exclude,FindObjectsSortMode.None));
            foreach (var farmer in ai_farmers)
            {
                farmer.stateManager.OnChangeState += (AI_Farmer_BaseState newState) => Farmer_OnChangeState(newState,farmer);
            }
        }
        public void UnsubscribeToFarmersStateChange()
        {
            foreach (var farmer in ai_farmers)
            {
                farmer.stateManager.OnChangeState -= (AI_Farmer_BaseState newState) => Farmer_OnChangeState(newState, farmer);
            }
        }
        private void Farmer_OnChangeState(AI_Farmer_BaseState newFarmerState, AI_Farmer_Dependencies origin)
        {
            if (AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_DeployCropsState)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_FindEmptyCropGroundState)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_FindGrownCrop)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_GatherCropsState))
                )
            {
                //has been prev added to crop job
                var indexToRemove = farmers_inSeedJob.FindIndex(x => x.ID == origin.idendity.ID);
                if (indexToRemove != -1)
                {
                    farmers_inSeedJob.RemoveAt(indexToRemove);
                }
                //has crop job
                if (farmers_inCropJob.Find(x => x.ID == origin.idendity.ID) == null)
                {
                    farmers_inCropJob.Add(origin.idendity);
                }

            }

            if (AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_DeploySeedsState)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_GatherSeedsState)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_FindEmptyCropGroundState)) ||
                AI_Summary.IsParentClass(newFarmerState.GetType(), typeof(AI_Farmer_PlantSeedState))
                )
            {
                //has been prev added to crop job
                var indexToRemove = farmers_inCropJob.FindIndex(x => x.ID == origin.idendity.ID);
                if (indexToRemove != -1)
                {
                    farmers_inCropJob.RemoveAt(indexToRemove);
                }
                //has seed job
                if (farmers_inSeedJob.Find(x => x.ID == origin.idendity.ID) == null)
                {
                    farmers_inSeedJob.Add(origin.idendity);
                }
            }
        }
    }
}
