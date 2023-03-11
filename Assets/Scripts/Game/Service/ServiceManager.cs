using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Service
{
	public class ServiceManager : MonoSingleton<ServiceManager>, IDisposable, IServiceManager
	{
		/// <summary>換場景時不要移除</summary>
		protected override bool dontDestroyOnLoad => true;

		private Dictionary<Type, BaseService> _dictServices;
        private List<IMonoUpdatable> _listUpdate;
        private List<IMonoFixedUpdatable> _listFixedUpdate;
        private List<IMonoLateUpdatable> _listLateUpdate;
        private List<IDisposable> _listDispose;

        public void Setup()
		{
            _dictServices = new Dictionary<Type, BaseService>();
            _listUpdate = new List<IMonoUpdatable>();
			_listFixedUpdate = new List<IMonoFixedUpdatable>();
			_listLateUpdate = new List<IMonoLateUpdatable>();
            _listDispose = new List<IDisposable>();
        }

		/// <summary>
		/// 取得服務
		/// </summary>
		public T GetService<T>() where T : BaseService
		{
			Type type = typeof(T);
			if (!_dictServices.ContainsKey(type))
			{
				Debug.LogError($"No service exist: {type.Name}");
				return null;
			}

			return _dictServices[type] as T;
		}

        /// <summary>
        /// 註冊新服務
        /// </summary>
        public void RegisterService<T>(T service, string[] args = null) where T : BaseService
        {
            Type serviceType = typeof(T);
            if (_dictServices.TryGetValue(serviceType, out BaseService duplicatedService))
                throw new InvalidOperationException($"Duplicated service: {serviceType}");

            _dictServices[serviceType] = service;

            // check interface supported
            IInitializable initializable = service as IInitializable;
            if (initializable != null)
            {
                initializable.Initialize(args);
            }
            IMonoUpdatable updatable = service as IMonoUpdatable;
            if (updatable != null)
            {
                _listUpdate.Add(updatable);
            }
            IMonoFixedUpdatable fixedUpdatable = service as IMonoFixedUpdatable;
            if (fixedUpdatable != null)
            {
                _listFixedUpdate.Add(fixedUpdatable);
            }
            IMonoLateUpdatable lateUpdatable = service as IMonoLateUpdatable;
            if (lateUpdatable != null)
            {
                _listLateUpdate.Add(lateUpdatable);
            }
            IDisposable disposable = service as IDisposable;
            if (disposable != null)
            {
                _listDispose.Add(disposable);
            }
        }

        #region implement IServiceManager
        public void OnEnterGame()
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnEnterGame();
            }

            foreach (var service in _dictServices.Values)
            {
                service.OnAfterEnterGame();
            }
        }
        public void OnLeaveGame()
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnLeaveGame();
            }
        }
        public void OnSceneChange()
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnSceneChange();
            }
        }
        public void OnSceneChanged()
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnSceneChanged();
            }
        }
        #endregion

        #region implement mono
        void Update()
        {
            foreach (IMonoUpdatable updator in _listUpdate)
            {
                updator.OnUpdate();
            }
        }

        void FixedUpdate()
        {
            foreach (IMonoFixedUpdatable updator in _listFixedUpdate)
            {
                updator.OnFixedUpdate();
            }
        }

        void LateUpdate()
        {
            foreach (IMonoLateUpdatable updator in _listLateUpdate)
            {
                updator.OnLateUpdate();
            }
        }

        void OnApplicationPause(bool pause)
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnApplicationPause(pause);
            }
        }

        void OnApplicationQuit()
        {
            foreach (var service in _dictServices.Values)
            {
                service.OnApplicationQuit();
            }
        }
        #endregion

        #region implement IDisposable
        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                foreach (var dispose in _listDispose)
                {
                    dispose.Dispose();
                }
                _dictServices = null;
                _listUpdate = null;
                _listFixedUpdate = null;
                _listLateUpdate = null;
            }
        }
        #endregion
    }
}
