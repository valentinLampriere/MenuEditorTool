namespace MenuGraph
{
	using UnityEngine;

	public abstract class MonoSingleton<t_manager> : MonoBehaviour where t_manager : MonoSingleton<t_manager>
	{
		#region Fields
		private static t_manager _instance = null;
		#endregion Fields

		#region Properties
		public static bool HasInstance { get { return _instance != null; } }
		public static t_manager Instance
		{
			get
			{
				if (_instance == null)
				{
					t_manager instance = GameObject.FindFirstObjectByType<t_manager>();
					
					if (instance == null)
					{
						Debug.LogError($"Couldn't find a {typeof(t_manager).Name} in the scene.");
						return null;
					}
					
					_instance.OnInitialize();
				}

				return _instance;
			}
		}
		#endregion Properties

		#region Methods

		protected virtual void OnDestroy()
		{ }

		protected virtual void OnInitialize()
		{ }
		#endregion Methods
	}
}