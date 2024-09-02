using CropField;
using CropField.Crops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_PlantState : AI_Farmer_BaseState
    {
        private CropBase currentHoldingCropSeeds => CropDataBase.instance.tomato;
        public override IEnumerator OnEnterState(AI_Farmer_StateManager stateManager)
        {
           
            var foundedCropHole = CropField_Manager.instance.GetClosestEmptyCropGround(stateManager.transform);
            if(foundedCropHole == null)
            {
                TextPopup(stateManager.transform.position + new Vector3(0f, 10f, 0f), "all crops are planted waiting...", Color.red, duration: 1f);
                yield return null;
            }
            else
            {
                TextPopup(stateManager.transform.position + new Vector3(0f, 10f, 0f), "planting seed", Color.green, duration: 1f);
            }
            yield return new WaitForSeconds(1f);

            foundedCropHole.AddSeedToHole(currentHoldingCropSeeds);
            stateManager.SetState(stateManager.state_FindEmptyCropGround);
        }

        public override void OnExitState(AI_Farmer_StateManager stateManager)
        {
            
        }

        public override void OnUpdateState(AI_Farmer_StateManager stateManager)
        {
            
        }
    }

}
