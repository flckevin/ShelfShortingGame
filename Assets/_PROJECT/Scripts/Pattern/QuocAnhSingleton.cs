using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Quocanh.pattern
{
    public class QuocAnhSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected bool dontDestroyOnLoad = false;
        private static T _instance;
        private static object _lock = new object();
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var instances = FindObjectsOfType<T>();
                        if (instances.Length > 0)
                        {
                            _instance = instances[0];
                            return _instance;
                        }
                    }


                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                        //DontDestroyOnLoad(_instance);
                    }
                    return _instance;
                }
            }
        }


        protected virtual void Start()
        {


            if (_instance != this && _instance != null)
            {
                Destroy(this.gameObject);
                return;
            }


            if (dontDestroyOnLoad == true)
            {
                DontDestroyOnLoad(this);
            }


            _instance = this as T;
        }


    }


}



