using System.Collections;
using System.Collections.Generic;
using Model.TileMap;
using Test;
using UnityEngine;
using UnityEngine.UI;

public class LeminCell : MonoBehaviour
{
	public Lemin lemin;
	public Vector2 pos;
	public Lemin.ECaptured type;

	public Image img;

	public void Init(Lemin lemin, Vector2 pos)
	{
		this.lemin = lemin;
		this.pos = pos;
	}

	public void OnClick_Press()
	{
		lemin.UpdateLeminImg(this);
	}
}
