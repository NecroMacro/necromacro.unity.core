using UnityEngine;
using UnityEngine.UI;

namespace NecroMacro.MVP
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic))]
    public class GradientGraphic : MonoBehaviour, IMeshModifier
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        public Direction direction;
        public Gradient gradient = new();

        private Graphic graphic;

        private void OnValidate()
        {
            UpdateSetup(true);
        }

        public void UpdateSetup(bool force = false)
        {
            if(graphic == null || force)
                graphic = GetComponent<Graphic>();

            if (graphic != null)
                graphic.SetVerticesDirty();
        }

        private float GetPosition(UIVertex vtx)
        {
            switch (direction)
            {
                case Direction.Vertical:
                    return vtx.position.y;
                default:
                    return vtx.position.x;
            }
        }

        public void ModifyMesh(VertexHelper verts)
        {
            if (!isActiveAndEnabled)
                return;

            var minPos = float.MaxValue;
            var maxPos = float.MinValue;

            var vtx = new UIVertex();
            for (int i = 0; i < verts.currentVertCount; ++i)
            {
                verts.PopulateUIVertex(ref vtx, i);
                minPos = Mathf.Min(minPos, GetPosition(vtx));
                maxPos = Mathf.Max(maxPos, GetPosition(vtx));
            }

            var delta = maxPos - minPos;
            if (delta == 0)
                return;
            
            for (int i = 0; i < verts.currentVertCount; ++i)
            {
                verts.PopulateUIVertex(ref vtx, i);

                var position = (GetPosition(vtx) - minPos) / delta;
                vtx.color *= gradient.Evaluate(position);
                verts.SetUIVertex(vtx, i);
            }
        }

        public void ModifyMesh(Mesh mesh)
        {
            //interface required
        }
    }
}
