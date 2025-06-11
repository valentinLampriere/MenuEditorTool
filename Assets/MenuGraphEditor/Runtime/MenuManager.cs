namespace MenuGraph
{
	using System.Collections.Generic;
	using UnityEditor.Overlays;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public sealed class MenuManager : MonoSingleton<MenuManager>
	{
		#region Enums
		private enum OpenFirstMenuMode
		{
			OnAwake,
			OnStart,
			None
		}
		#endregion Enums

		#region Fields
		[SerializeField] private MenuGraph _menuGraph = null;
		[SerializeField] private OpenFirstMenuMode _openFirstMenuMode = OpenFirstMenuMode.OnStart;
		[SerializeField] private InputActionReference _cancelActionReference = null;

		private MenuUI _currentMenu = null;
		private MenuNode _currentMenuNode = null;
		private Dictionary<int, MenuUI> _instantiatedMenus = null;
		#endregion Fields

		#region Methods
		private void Awake()
		{
			_instantiatedMenus = new Dictionary<int, MenuUI>();

			if (_openFirstMenuMode == OpenFirstMenuMode.OnAwake)
			{
				OpenFirstMenu();
			}

			_cancelActionReference.action.Enable();
			_cancelActionReference.action.performed += OnCancelPerformed;
		}

		private void Start()
		{
			if (_openFirstMenuMode == OpenFirstMenuMode.OnStart)
			{
				OpenFirstMenu();
			}
		}

		protected override void OnDestroy()
		{
			if (_cancelActionReference != null && _cancelActionReference.action != null)
			{
				_cancelActionReference.action.performed -= OnCancelPerformed;
			}

			_currentMenu = null;
			_currentMenuNode = null;

			if (_instantiatedMenus != null)
			{
				foreach (KeyValuePair<int, MenuUI> instantiatedMenu in _instantiatedMenus)
				{
					GameObject.Destroy(instantiatedMenu.Value.gameObject);
				}
			}

			base.OnDestroy();
		}

		private void OpenMenu(MenuNode menuNode)
		{
			if (_currentMenu != null)
			{
				_currentMenu.gameObject.SetActive(false);
				_currentMenu.NextMenuRequested -= OnNextMenuRequested;
				_currentMenu.OnClosed();
			}

			int menuId = menuNode.TargetMenu.GetInstanceID();
			if (_instantiatedMenus.TryGetValue(menuId, out MenuUI menu) == false)
			{
				menu = GameObject.Instantiate(menuNode.TargetMenu, transform);
				menu.OnCreated();
				_instantiatedMenus.Add(menuId, menu);
			}

			menu.gameObject.SetActive(true);
			menu.NextMenuRequested += OnNextMenuRequested;
			menu.OnOpened();
			_currentMenu = menu;
			_currentMenuNode = menuNode;
		}

		private void OpenFirstMenu()
		{
			MenuNode rootMenuNode = _menuGraph.RootMenuNode;
			OpenMenu(rootMenuNode);
		}

		private void OnNextMenuRequested(int actionIndex)
		{
			if (_currentMenuNode.TryGetNextMenuNode(actionIndex, out MenuNode nextMenuNode) == true)
			{
				OpenMenu(nextMenuNode);
			}
		}

		private void OnCancelPerformed(InputAction.CallbackContext context)
		{
			if (_currentMenuNode.Parent != null)
			{
				OpenMenu(_currentMenuNode.Parent);
			}
		}
		#endregion Methods
	}
}