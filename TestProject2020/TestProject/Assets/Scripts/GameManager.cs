using Cysharp.Threading.Tasks;
using EuNet.Core;
using EuNet.Unity;
using UnityEngine;

[ExecutionOrder(-10)]
[RequireComponent(typeof(NetView))]
public class GameManager : SceneSingleton<GameManager>, INetViewHandler
{
    public Actor ControlActor { get; set; }
    private NetView _view;

    protected override void Awake()
    {
        base.Awake();

        _view = GetComponent<NetView>();
    }

    private async UniTaskVoid Start()
    {
        if (NetClientGlobal.Instance.MasterIsMine() == false)
        {
            // 마스터가 아니므로 마스터로부터 현재 게임 상황을 받아서 복구시킴
            await RecoveryAsync();
        }

        CreateMyPlayer();
    }

    public async UniTask<bool> RecoveryAsync()
    {
        await UniTask.DelayFrame(1);
        await NetClientGlobal.Instance.RequestRecovery();
        return true;
    }

    private void CreateMyPlayer()
    {
        NetPool.DataWriterPool.Use((writer) =>
        {
            // 내가 주인인 플레이어를 생성함
            var playerObj = NetClientGlobal.Instance.Instantiate(
                "Player",
                new Vector3(Random.Range(-1f, 1f), 0.5f, 0f),
                Quaternion.identity,
                writer);

            // 컨트롤을 할 수 있도록 등록
            ControlActor = playerObj.GetComponent<Actor>();
        });
    }

    public void OnViewInstantiate(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public void OnViewDestroy(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public void OnViewMessage(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }
}
