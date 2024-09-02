using CropField;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DebugHelper.DebugHelper;
using Random = UnityEngine.Random;
namespace AI.Farmer
{
    public class AI_Farmer_FindEmptyCropGroundState : AI_Farmer_BaseState
    {
       private const int pointsPerCurve = 4;
       private LayerMask groundLayer = 1 << 3;
       private Ease movementEase = Ease.OutSine;
        public override IEnumerator OnEnterState(AI_Farmer_StateManager stateManager)
        {
            Transform transform = stateManager.transform;
            Debug.Log("Entered patrol state");
            
            stateManager.pathfinding.StartBezierPath(
                startWorldPosition: transform.position,
                destinationWorldPosition: FindEmptyCropSpace(stateManager),
                OnSuccessGeneratingPath:(List<Vector3> worldPositionPoints) => 
                {
                    TextPopup(transform.position + new Vector3(0f, 10f, 0f), "Searching for crop holes", Color.white, duration: 2f);
                    MoveByPoints(transform,stateManager,worldPositionPoints,
                    OnCompleteMoving: () =>
                    {
                        stateManager.SetState(stateManager.state_plantSeed);
                    });
                },
                OnFailed:() =>
                {
                    TextPopup(transform.position + new Vector3(0f, 10f, 0f), "Something went wrong with generating path!", Color.white, duration: 2f);
                },
                pointsPerCurve: pointsPerCurve);
            yield return null;
        }

        public override void OnExitState(AI_Farmer_StateManager stateManager)
        {
            
        }

        public override void OnUpdateState(AI_Farmer_StateManager stateManager)
        {
            Debug.Log("Updating patrol state");
        }
        private void MoveByPoints(Transform moveTarget, AI_Farmer_StateManager stateManager, List<Vector3> worldPositionPoints, Action OnCompleteMoving)
        {
            ///Convert pathfinding V3 world points to V3 world points based on ground elevation
            Vector3[] path = worldPositionPoints.Select(v3 =>
            {
                if (Physics.Raycast(v3, direction: Vector3.down, maxDistance: 100f, hitInfo: out RaycastHit hit, layerMask: groundLayer))
                {
                    return hit.point;
                }
                else
                {
                    return new Vector3(v3.x, moveTarget.position.y, v3.z);
                }
;
            }).ToArray();
            moveTarget.DOKill();
            moveTarget.DOPath(path, duration: worldPositionPoints.Count * (0.25f / stateManager.actorStatistics.mvSpeed), pathType: PathType.Linear, gizmoColor: new Color(0f, 0f, 0f, 0f))
                .SetEase(movementEase)
                .OnComplete(() => OnCompleteMoving?.Invoke());
        }
        private Vector3 FindEmptyCropSpace(AI_Farmer_StateManager stateManager)
        {
            if (CropField_Manager.instance.cropFields.Count == 0)
                return default;
            return CropField_Manager.instance.GetClosestEmptyCropGroundPosition(stateManager.transform);
        }
    }
}
