using Model.Photon;
using UnityEngine;

namespace Model.Entity
{
    public class PlayerChecker : MonoBehaviour
    {
        [SerializeField] private CustomRaiseEvents customRaiseEvents;
        [SerializeField] private PlayerMove player;
        private Transform playerBody;

        public void Init(Transform body)
        {
            playerBody = body;
        }
        
        public void StartBattle()
        {
            customRaiseEvents.UpdateTileMapGhost_Action += UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action += UpdateCapture;
        }

        public void EndBattle()
        {
            customRaiseEvents.UpdateTileMapGhost_Action -= UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action -= UpdateCapture;
        }

        private void UpdateGhost(Vector3Int vector, int playerId)
        {
            if (Vector3.Distance(vector, playerBody.position) < 1)
            {
                player.GameOver(false);
            }
        }

        private void UpdateCapture(Vector3Int[] vectors, int playerId)
        {
            for (int x = 0; x < vectors.Length; x++)
            {
                if (Vector3.Distance(vectors[x], playerBody.position) < 1)
                {
                    player.GameOver(false);
                }
            }
        }
    }
}