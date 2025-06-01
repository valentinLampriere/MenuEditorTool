namespace MenuGraph.Editor
{
	using UnityEngine.UIElements;

	/// <summary>
	/// This is just a <see cref="UnityEngine.UIElements.TwoPaneSplitView"/> which can be displayed in the UXML editor.
	/// </summary>
	[UxmlElement]
	public sealed partial class TwoPaneSplitViewUxml : UnityEngine.UIElements.TwoPaneSplitView
	{
		public TwoPaneSplitViewUxml() : base()
		{
			contentContainer.Clear();
		}
	}
}