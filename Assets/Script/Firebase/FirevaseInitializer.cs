using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̾�̽� �ۿ��� ���� �÷��̿� ���� �������� �ִ� �͵��� ����� ������ �Ǿ����� Ȯ�� �� �˷���. 

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
