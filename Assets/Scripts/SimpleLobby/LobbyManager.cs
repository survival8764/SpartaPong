using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks //클래스 상속
{
    [SerializeField] private TextMeshProUGUI StatusText; 
    [SerializeField] private GameObject waitPanel;

    private readonly int MAX_USERS = 2;
    private bool connect = false;
    
    //현재 상태 표시 
    private void Update() => StatusText.text = "Current Status: " + PhotonNetwork.NetworkClientState.ToString();

    //서버에 접속
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    
    //연결 끊기
    public void Disconnect() => PhotonNetwork.Disconnect();
    
    //연결 되면 호출
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server");
        connect = true;
    }
    
    //연결 끊겼을 때 호출
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("Disconnected");

    //방 입장
    public void JoinRoom()
    {
        if(connect)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log( "Entered");
        }
        else
        {
            Debug.LogWarning("Not Connected");
        }
    }

    //랜덤 룸 입장에 실패하면 새로운 방 생성 (master 방 생성)
    public override void OnJoinRandomFailed(short returnCode, string message) =>
        PhotonNetwork.CreateRoom(Random.Range(1,1000).ToString(), new RoomOptions { MaxPlayers = MAX_USERS });

    //방에 입장 했을 때 호출 
    public override void OnJoinedRoom()
    {
        int currentPlayers = PhotonNetwork.CurrentRoom.Players.Count;

        // 현재 방에 있는 플레이어 수에 따라 조건을 분기할 수 있습니다.
        // 예를 들어, 특정 수의 플레이어가 필요한 게임 시작 로직을 구현할 수 있습니다.
        if(currentPlayers == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            // 대기 필요, 더 많은 플레이어를 기다리는 로직
            Debug.Log(currentPlayers + " / " + PhotonNetwork.CurrentRoom.MaxPlayers + " players joined. Waiting for more...");
            // 대기 UI 활성화 또는 기타 로직 구현
            waitPanel.SetActive(true); // 가정: uiPanel이 대기중인 플레이어를 위한 UI라고 가정합니다.
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 더이상 들어오지 않도록 막음.
        PhotonNetwork.CurrentRoom.IsOpen = false;
        // 두 명이서 하는 게임으로, 더이상 체크할 필요 없음.
        waitPanel.SetActive(false);
        PhotonNetwork.LoadLevel(1);
    }
}