using Astar.Brain;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using System.Linq;
using static DebugHelper.DebugHelper;
namespace Actor
{
    public class PointNClickActor_AstarMovement : MonoBehaviour
    {
        [SerializeField] private AstarPathfinding pathfinding;
        [SerializeField] private Transform moveTarget;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private ActorStatistics statistics;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private AstarPathType pathType;
        [SerializeField] private int pointsPerCurve = 4;
        private Coroutine processMove;
        enum AstarPathType
        {
            Linear,
            Bezier
        }
        private void OnEnable()
        {
            if (moveTarget == null)
                moveTarget = transform;
            InputHandler.instance.onPrimaryBtnTap += OnPrimaryBtnTap;
        }

        private void OnDisable()
        {
            InputHandler.instance.onPrimaryBtnTap -= OnPrimaryBtnTap;
        }
        private void OnPrimaryBtnTap(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            TextPopup(transform.position + new Vector3(0f,10f,0f),"started path", Color.white,duration:2f);
            switch (pathType)
            {
                case AstarPathType.Linear:
                        pathfinding.StartLinearPath(transform.position, mouseWorldPos,
                        OnFailed:() =>
                        {
                            Debug.Log("Destination path is impossible");
                        }, 
                        OnSuccessGeneratingPath: (List<Vector3> worldPointsPosition) =>
                        {
                            MoveByPoints(worldPointsPosition);
                        });
                    break;
                case AstarPathType.Bezier:
                        pathfinding.StartBezierPath(transform.position, mouseWorldPos,
                        OnFailed:() =>
                        {
                            Debug.Log("Destination path is impossible");
                        }, 
                        OnSuccessGeneratingPath: (List<Vector3> worldPointsPosition) =>
                        {
                            MoveByPoints(worldPointsPosition);
                        },pointsPerCurve: pointsPerCurve);
                    break;
                default:
                    break;
            }
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
