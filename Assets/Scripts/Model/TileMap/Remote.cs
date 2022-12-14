using Model.Photon;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
    public class Remote : MonoBehaviour
    {
        [SerializeField] private CustomRaiseEvents customRaiseEvents;
        [SerializeField] private TileDataBase tileDataBase;

        [SerializeField] private Tilemap remote, local;

        private void OnEnable()
        {
            customRaiseEvents.UpdateTileMapGhost_Action += UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action += UpdateCapture;
        }

        private void OnDisable()
        {
            customRaiseEvents.UpdateTileMapGhost_Action -= UpdateGhost;
            customRaiseEvents.UpdateTileMapCapture_Action -= UpdateCapture;
        }

        private void UpdateGhost(Vector3Int vector, int playerId)
        {
            remote.SetTile(vector, tileDataBase.tiles[playerId]);
            remote.SetTileFlags(vector, TileFlags.None);
            remote.SetColor(vector, Color.yellow);
        }

        private void UpdateCapture(Vector3Int[] vectors, int playerId)
        {
            for (int x = 0; x < vectors.Length; x++)
            {
                local.SetTile(vectors[x], tileDataBase.tiles[playerId]);
                local.SetTileFlags(vectors[x], TileFlags.None);
                local.SetColor(vectors[x], Color.white);
                
                remote.SetTile(vectors[x], null);
                remote.SetTileFlags(vectors[x], TileFlags.None);
                remote.SetColor(vectors[x], Color.white);
            }
        }
    }
}
