using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Ball")]
    public Ball ball;

    [Header("Player 1")]
    public Paddle player1Paddle;
    public Goal player1Goal;

    [Header("Player 2")]
    public Paddle player2Paddle;
    public Goal player2Goal;

    [Header("UI")]
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    private int player1Score;
    private int player2Score;

    private void Start()
    {
        SpawnPaddle();
        if(photonView.AmOwner)
            SpawnBall();
    }

    private void SpawnPaddle()
    {
        int idx = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject prefab = Resources.Load<GameObject>("Paddle");

        if(idx == 1)
        {
            PhotonNetwork.Instantiate(prefab.name, new Vector3(-12, 0, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(prefab.name, new Vector3(12, 0, 0), Quaternion.identity);
        }
    }

    private void SpawnBall()
    {
        GameObject prefab = Resources.Load<GameObject>("Ball");
        GameObject go = PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
        ball = go.GetComponent<Ball>();

    }


    public void Player1Scored()
    {
        if (photonView.AmOwner)
        {
            player1Score++;
            ResetPosition();
            photonView.RPC(nameof(UpdateScore), RpcTarget.All, player1Score, player2Score);
        }
    }

    public void Player2Scored()
    {
        if (photonView.AmOwner)
        {
            player2Score++;
            ResetPosition();
            photonView.RPC(nameof(UpdateScore), RpcTarget.All, player1Score, player2Score);
        }
    }
    

    [PunRPC]
    public void UpdateScore(int score1, int score2)
    {
        player1Text.text = score1.ToString();
        player2Text.text = score2.ToString();

        if (score1 > 5 || score2 > 5)
            PhotonNetwork.LeaveRoom();
    }

    private void ResetPosition()
    {
        ball.Reset();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


}