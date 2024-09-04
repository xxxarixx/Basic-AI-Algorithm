using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace General.Containers
{
    [DefaultExecutionOrder(-1)]
    public class ChestsManager : MonoBehaviour
    {
        private List<Chest> chests = new List<Chest>();
        public static ChestsManager instance { get; private set; }
        public bool settuped = false;
        public Chest FindNearestChest(Actor_Idendity target)
        {
            if(chests.Count == 0)
                return default;
            float closestDist = Mathf.Infinity;
            Chest closestChest = null;
            foreach (Chest chest in chests)
            {
                var dist = Vector3.Distance(target.transform.position, chest.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestChest = chest;
                }
            }
            if (closestChest == null)
                return default;
            return closestChest;
            
        }
        private void Awake()
        {
            chests.AddRange(FindObjectsByType<Chest>(sortMode: FindObjectsSortMode.None, findObjectsInactive: FindObjectsInactive.Exclude));
            instance = this;
            settuped = true;
        }
        public static void AddChestToPool(Chest chest)
        {
            if (instance == null)
                return;
            instance.chests.Add(chest);
        }
    }
}
