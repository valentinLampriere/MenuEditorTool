namespace MenuGraph.Editor
{
	using System;
	using System.Collections.Generic;
	using Object = UnityEngine.Object;

	internal abstract class ISelectionWatcherComponent : IDisposable
	{
		#region Fields
		private Dictionary<Type, List<Delegate>> _registeredCallbacks = null;
		#endregion Fields

		#region Constructors
		internal ISelectionWatcherComponent()
		{
			_registeredCallbacks = new Dictionary<Type, List<Delegate>>();
		}
		#endregion Constructors

		#region Methods
		#region IDisposable
		public virtual void Dispose()
		{
			if (_registeredCallbacks != null)
			{
				foreach (KeyValuePair<Type, List<Delegate>> callbacksByType in _registeredCallbacks)
				{
					callbacksByType.Value.Clear();
				}

				_registeredCallbacks.Clear();
				_registeredCallbacks = null;
			}
		}
		#endregion IDisposable

		#region APIs
		internal void Register<t_type>(Action<t_type> callback) where t_type : Object
		{
			Type type = typeof(t_type);

			if (_registeredCallbacks.ContainsKey(type) == false)
			{
				_registeredCallbacks.Add(type, new List<Delegate>());
			}

			_registeredCallbacks[type].Add(callback);
		}

		internal void Unregister<t_type>(Action<t_type> callback) where t_type : Object
		{
			Type type = typeof(t_type);

			if (_registeredCallbacks.TryGetValue(type, out List<Delegate> callbacks) == true)
			{
				callbacks.Remove(callback);

				if (callbacks.Count == 0)
				{
					_registeredCallbacks.Remove(type);
				}
			}
		}

		internal void Evaluate(Object[] selectedObjects)
		{
			IEnumerable<Object> targetObjects = RetrieveObjects(selectedObjects);
			foreach (Object targetObject in targetObjects)
			{
				EvaluateCallbacks(targetObject);
			}
		}

		internal abstract IEnumerable<Object> RetrieveObjects(Object[] selectedObjects);
		#endregion APIs

		#region Privates
		private void EvaluateCallbacks(Object targetObject)
		{
			Type targetObjectType = targetObject.GetType();

			foreach (KeyValuePair<Type, List<Delegate>> callbacksByType in _registeredCallbacks)
			{
				Type type = callbacksByType.Key;

				if (type.IsAssignableFrom(targetObjectType) == false)
				{
					continue;
				}

				InvokeCallbacks(callbacksByType.Value, targetObject);
			}
		}

		private void InvokeCallbacks(List<Delegate> callbacks, Object targetObject)
		{
			int callbacksCount = callbacks.Count;
			for (int i = 0; i < callbacksCount; i++)
			{
				Delegate callback = callbacks[i];

				if (callback is Action<Object> genericCallback)
				{
					genericCallback(targetObject);
				}
				else
				{
					callback.DynamicInvoke(targetObject);
				}
			}
		}
		#endregion Privates
		#endregion Methods
	}
}