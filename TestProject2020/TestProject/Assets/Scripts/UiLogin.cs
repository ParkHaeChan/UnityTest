using Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using EuNet.Unity;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLogin : MonoBehaviour
{
    [SerializeField] private Button _loginButton;

    private void Start()
    {
        _loginButton.OnClickAsAsyncEnumerable().Subscribe(OnLoginAsync);

    }

    private async UniTaskVoid OnLoginAsync(AsyncUnit asyncUnit)
    {
        try
        {
            _loginButton.interactable = false;

            var client = NetClientGlobal.Instance.Client;

            var result = await client.ConnectAsync(TimeSpan.FromSeconds(10));

            if (result == true)
            {
                LoginRpc loginRpc = new LoginRpc(client);
                var loginResult = await loginRpc.Login(SystemInfo.deviceUniqueIdentifier);

                Debug.Log($"Login Result : {loginResult}");
                if (loginResult != 0)
                    return;

                var joinResult = await loginRpc.Join();
                Debug.Log($"Join : {joinResult}");

                // Game Scene 으로 이동
                await SceneManager.LoadSceneAsync("PhysicsSample");
            }
            else
            {
                Debug.LogError("Fail to connect server");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            if (_loginButton != null)
                _loginButton.interactable = true;
        }
    }
}