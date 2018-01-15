using UnityEngine;
using UnityEditor;
using System.Collections;

namespace HighlightingSystem
{
	[CustomEditor(typeof(HighlightingRenderer))]
	public class HighlightingRendererEditor : HighlightingBaseEditor
	{
		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Highlight Color:");
			HighlightingRenderer hr = target as HighlightingRenderer;
			hr.HighlightColor = EditorGUILayout.ColorField(hr.HighlightColor);
			GUILayout.EndHorizontal ();
			CommonGUI();
		}
	}
}