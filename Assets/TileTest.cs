using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class TileTest : MonoBehaviour {
    public Tilemap tilemap;

    private void onMouseOver() {
        try {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);



            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);



            if (this.tilemap = hit.transform.GetComponent<Tilemap>()) {
                this.tilemap.RefreshAllTiles();

                int x, y;
                x = this.tilemap.WorldToCell(ray.origin).x;
                y = this.tilemap.WorldToCell(ray.origin).y;

                Vector3Int v3Int = new Vector3Int(x, y, 0);



                //타일 색 바꿀 때 이게 있어야 하더군요
                this.tilemap.SetTileFlags(v3Int, TileFlags.None);

                //타일 색 바꾸기
                this.tilemap.SetColor(v3Int, (Color.red));
                Debug.Log("On Block");

            }
        } catch (NullReferenceException) {

        }
    }
    private void onMouseExit() {
        this.tilemap.RefreshAllTiles();

    }
}
