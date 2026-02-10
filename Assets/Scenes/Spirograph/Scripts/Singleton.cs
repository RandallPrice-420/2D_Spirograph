using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public  static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject(name: "Instance of " + typeof(T)).AddComponent<T>();
                }
            }

            return _instance;
        }
    }


}   // class Singleton<T>
