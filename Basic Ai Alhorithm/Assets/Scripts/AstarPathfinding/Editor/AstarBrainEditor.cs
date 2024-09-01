using Astar.Brain;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
namespace Astar.Brain
{
    [CustomEditor(typeof(AstarBrain))]
    public class AstarBrainEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement myInspector = new VisualElement();
            InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);
            AstarBrain astarBrain = target as AstarBrain;
            Button btn = new Button(clickEvent: () =>
            {
                astarBrain.Scan();
            });
            btn.text = $"Scan";
            myInspector.Add(btn);
            return myInspector;
        }
    }    
}
