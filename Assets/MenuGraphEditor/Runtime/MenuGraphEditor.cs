#if UNITY_EDITOR
namespace MenuGraph
{
	using UnityEditor;

	public sealed partial class MenuGraph
	{
		#region Methods
		public void AddMenuNode(MenuNode menuNode)
		{
			_menuNodes.Add(menuNode);
		}

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