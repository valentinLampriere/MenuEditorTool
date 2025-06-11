namespace MenuGraph
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.UI;

	// TODO : Bad name.
	public partial class MenuUI : MonoBehaviour
	{
		#region Inner Classes
		[Serializable]
		// TODO : Improve menu action + move this outside?
		public sealed class MenuNodeAction
		{
			#region Fields
			[SerializeField] private string _editorName = null;
			[SerializeField] private InputActionReference _inputActionReference = null;
			//[SerializeField] private InputAction _inputAction = null;
			[SerializeField] private Button _button = null;
			#endregion Fields

			#region Properties
			public string EditorName { get { return _editorName; } }
			#endregion Properties

			#region Events
			private Action _requestNextEvent = null;
			public event Action RequestNext
			{
				add
				{
					_requestNextEvent -= value;

					RegisterEvent(_requestNextEvent);
					_requestNextEvent += value;
				}
				remove
				{
					UnregisterEvents();

					_requestNextEvent -= value;
				}
			}
			#endregion Events

			#region Methods
			public bool IsValid()
			{
				return _inputActionReference != null || _button != null;
			}

			private void RegisterEvent(Action action)
			{
				if (_button != null)
				{
					_button.onClick.AddListener(OnButtonClicked);
				}
				else if (_inputActionReference != null)
				{
					if (_inputActionReference.action.enabled == false)
					{
						_inputActionReference.action.Enable();
					}

					_inputActionReference.action.performed += OnInputActionPerformed;
				}
			}

			private void UnregisterEvents()
			{
				if (_button != null)
				{
					_button.onClick.RemoveListener(OnButtonClicked);
				}
				else if (_inputActionReference != null)
				{
					_inputActionReference.action.Disable();
					_inputActionReference.action.performed -= OnInputActionPerformed;
				}
			}

			private void OnButtonClicked()
			{
				_requestNextEvent?.Invoke();
			}

			private void OnInputActionPerformed(InputAction.CallbackContext context)
			{
				_requestNextEvent?.Invoke();
			}
			#endregion Methods
		}
		#endregion Inner Classes

		#region Fields
		[SerializeField] private List<MenuNodeAction> _menuActions = null;
		#endregion Fields

		#region Properties
		// TODO: Could be private and add a function instead.
		public IReadOnlyList<MenuNodeAction> MenuActions { get { return _menuActions; } }
		#endregion Properties

		#region Events
		private Action<int> _nextMenuRequestedEvent = null;
		public event Action<int> NextMenuRequested
		{
			add
			{
				_nextMenuRequestedEvent -= value;
				_nextMenuRequestedEvent += value;
			}
			remove
			{
				_nextMenuRequestedEvent -= value;
			}
		}
		#endregion Events

		#region Methods
		private void Awake()
		{
			int menuActionsCount = _menuActions.Count;
			for (int i = 0; i < menuActionsCount; i++)
			{
				MenuNodeAction menuAction = _menuActions[i];
				menuAction.RequestNext += () =>
				{
					int actionIndex = _menuActions.FindIndex((MenuNodeAction menuNodeAction) => { return menuAction == menuNodeAction; });
					_nextMenuRequestedEvent?.Invoke(actionIndex);
				};
			}
		}
		
		public virtual void OnCreated()
		{

		}
		
		public virtual void OnOpened()
		{

		}

		public virtual void OnClosed()
		{

		}
		#endregion Methods
	}
}