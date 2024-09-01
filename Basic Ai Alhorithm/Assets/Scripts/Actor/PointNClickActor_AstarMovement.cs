using Astar.Brain;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using System.Linq;
namespace Actor
{
    public class PointNClickActor_AstarMovement : MonoBehaviour
    {
        [SerializeField] private AstarPathfinding pathfinding;
        [SerializeField] private Transform moveTarget;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private ActorStatistics statistics;
        [SerializeField] private Ease ease = Ease.Linear;
        private Coroutine processMove;
        private void OnEnable()
        {
            if (moveTarget == null)
                moveTarget = transform;
            InputHandler.instance.onPrimaryBtnPressed += Instance_onPrimaryBtnPressed;
        }

        private void OnDisable()
        {
            InputHandler.instance.onPrimaryBtnPressed -= Instance_onPrimaryBtnPressed;
        }
        private void Instance_onPrimaryBtnPressed(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            pathfinding.StartPath(transform.position, mouseWorldPos,
            OnFailed:() =>
            {
                Debug.Log("Destination path is impossible");
            }, 
            OnSuccessGeneratingPath: (List<Vector3> worldPointsPosition) =>
            {
                MoveByPoints(worldPointsPosition);
            });
        }

        private void MoveByPoints(List<Vector3> worldPositionPoints)
        {
            ///Convert pathfinding V3 world points to V3 world points based on ground elevation
            Vector3[] path = worldPositionPoints.Select(v3 =>
            {
                if (Physics.Raycast(v3, direction: Vector3.down,maxDistance:100f, hitInfo:out RaycastHit hit, layerMask:groundLayer))
                {
                    return hit.point;
                }
                else
                {
                    return new Vector3(v3.x,moveTarget.position.y,v3.z);
                }
;
            }).ToArray();
            moveTarget.DOKill();
            moveTarget.DOPath(path, duration: worldPositionPoints.Count * (0.25f / statistics.mvSpeed), pathType: PathType.Linear,gizmoColor:new Color(0f,0f,0f,0f)).SetEase(ease);
        }
    }

}
