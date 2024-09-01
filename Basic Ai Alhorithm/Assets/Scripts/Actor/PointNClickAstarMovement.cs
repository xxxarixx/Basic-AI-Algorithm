using Astar.Brain;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Actor
{
    public class PointNClickAstarMovement : MonoBehaviour
    {
        [SerializeField] private AstarPathfinding pathfinding;
        private Coroutine processMove;
        private void OnEnable()
        {
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
            if (processMove != null)
                StopCoroutine(processMove);
            processMove = StartCoroutine(ProcessMoveInPoints(worldPositionPoints));
        }
        IEnumerator ProcessMoveInPoints(List<Vector3> worldPositionPoints)
        {
            foreach (Vector3 point in worldPositionPoints)
            {
                
            }
            yield return null;
        }
    }

}
