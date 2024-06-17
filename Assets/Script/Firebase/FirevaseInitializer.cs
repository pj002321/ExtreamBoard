using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파이어베이스 앱에서 구글 플레이에 대한 의존성이 있는 것들이 제대로 정리가 되었는지 확인 후 알려줌. 

public class FirebaseInitializer : MonoBehaviour
{
    private static List<Action<DependencyStatus>> initailizeCallbacks = new List<Action<DependencyStatus>>();
    private static List<Action> activateFetchCallbacks = new List<Action>();
    private static DependencyStatus dependencyStatus;

    private static bool initialized = false;
    private static bool fetching = false;
    private static bool activateFetch = false;

    public static void Initialize(Action<DependencyStatus> callback)
    {
        lock (initailizeCallbacks)
        {
            if (initialized)
            {
                callback(dependencyStatus);
                return;
            }

            initailizeCallbacks.Add(callback);
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                lock (initailizeCallbacks)
                {
                    dependencyStatus = task.Result;
                    initialized = true;
                    CallInitializedCallbacks();
                }
            });
        }
    }

    public static void CallInitializedCallbacks()
    {
        lock (initailizeCallbacks)
        {
            foreach (var callback in initailizeCallbacks)
            {
                callback(dependencyStatus);
            }
            initailizeCallbacks.Clear();
        }
    }
}
