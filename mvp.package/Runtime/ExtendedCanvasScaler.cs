using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NecroMacro.UI
{
	[ExecuteInEditMode]
	public class ExtendedCanvasScaler : CanvasScaler
	{
		public void Refresh()
		{
			base.OnEnable();
		}

		protected override void OnEnable()
		{
			// блокируем дублирование обновление в энабле; метод выше должен быть обязательно вызван вручную
			if (!Application.isPlaying) base.OnEnable();
		}
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(ExtendedCanvasScaler), true)]
	internal class ExtendedCanvasScaler_Editor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}
	}
	#endif
}
