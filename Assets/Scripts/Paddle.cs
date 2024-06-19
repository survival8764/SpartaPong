using Photon.Pun;
using UnityEngine;

public class Paddle : MonoBehaviourPun
{
    public float speed;
    
    void Update()
    {
        if (!photonView.IsMine) return;
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(0, move, 0);
    }
}