using Model.Photon;
using Model.TileMap;
using Test;
using UnityEngine;

namespace Model.Entity
{
    public class PlayerChecker : MonoBehaviour
    {
        [SerializeField] private CustomRaiseEvents customRaiseEvents;
        [SerializeField] private PlayerMove player;
        private Transform playerBody;
        private LeminCell[][] cells;

        private TilemapInstance tilemapInstance;

        public void InitPlayer(Transform body)
        {
            playerBody = body;
        }

        public void InitMap(TilemapInstance tilemapInstance)
        {
            this.tilemapInstance = tilemapInstance;
            cells = tilemapInstance.GetCells;
        }

        public void StartBattle()
        {
            customRaiseEvents.UpdateTileMapGhost_Action += UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action += UpdateCapture;
            customRaiseEvents.AttackPlayer_Action += PlayerDamage;
        }

        public void EndBattle()
        {
            customRaiseEvents.UpdateTileMapGhost_Action -= UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action -= UpdateCapture;
            customRaiseEvents.AttackPlayer_Action -= PlayerDamage;
        }

        public void CheckAttack(Transform point)
        {
            Vector3Int pos = Vector3Int.FloorToInt(point.position);
            if (cells[pos.x][pos.y].type == Lemin.ECaptured.ghost)
            {
                customRaiseEvents.Request_AttackPlayer(tilemapInstance.GetTileId(pos));
            }
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

        private void PlayerDamage(int playerId)
        {
            if (player.PlayerId == playerId)
                player.GameOver(false);
        }
    }
}