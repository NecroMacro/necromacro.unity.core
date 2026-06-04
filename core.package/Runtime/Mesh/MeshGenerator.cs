using UnityEngine;

namespace NecroMacro.Core.Mesh
{
	public class MeshGenerator
	{
		public static UnityEngine.Mesh GenerateQuad()
		{
			var mesh = new UnityEngine.Mesh
			{
				vertices = new Vector3[]
				{
					new(-1, -1, 0),
					new( 1, -1, 0),
					new( 1,  1, 0),
					new(-1,  1, 0)
				},
				triangles = new[] { 0, 1, 2, 0, 2, 3 }
			};

			mesh.RecalculateNormals();

			return mesh;
		}
	}
}