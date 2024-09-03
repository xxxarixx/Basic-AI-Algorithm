using Astar.Brain;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DebugHelper.DebugHelper;

namespace AI.Farmer
{
    public class AI_Farmer_Dependencies : MonoBehaviour
    {
        public AI_Farmer_Inventory inventory;
        public AI_Farmer_StateManager stateManager;
        public AI_Farmer_Apperance apperance;
        public ActorStatistics actorStatistics;
        public AstarPathfinding pathfinding;
        public MeshRenderer mRenderer;

        private LayerMask groundLayer = 1 << 3;
        private Ease movementEase = Ease.OutSine;
        public void MoveByPoints(Transform moveTarget, List<Vector3> worldPositionPoints, Action OnCompleteMoving)
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
            moveTarget.DOPath(path, duration: worldPositionPoints.Count * (0.25f / actorStatistics.mvSpeed), pathType: PathType.Linear, gizmoColor: new Color(0f, 0f, 0f, 0f))
                .SetEase(movementEase)
                .OnComplete(() => OnCompleteMoving?.Invoke());
        }
        public void MoveByPathfindingToDestination(Vector3 destination, Action OnCompleteMoving)
        {
            pathfinding.StartBezierPath(
                startWorldPosition: transform.position,
                destinationWorldPosition: destination,
                OnSuccessGeneratingPath: (List<Vector3> worldPositionPoints) =>
                {
                    MoveByPoints(transform, worldPositionPoints,
                    OnCompleteMoving: () =>
                    {
                        OnCompleteMoving?.Invoke();
                    });
                },
                OnFailed: () =>
                {
                    TextPopup(transform.position + new Vector3(0f, 10f, 0f), "Something went wrong with generating path!", Color.red, duration: 1f);
                });
        }
    }
}
