#if UNITY_EDITOR
namespace MenuGraph
{
	using UnityEditor;
	using UnityEngine;

	public sealed partial class MenuGraph
	{
		#region Methods
		public void DeleteMenuNode(MenuNode menuNode)
		{
			_menuNodes.Remove(menuNode);
			AssetDatabase.RemoveObjectFromAsset(menuNode);
			AssetDatabase.SaveAssets();
		}
		#endregion Methods
	}
} 
#endif // UNITY_EDITOR