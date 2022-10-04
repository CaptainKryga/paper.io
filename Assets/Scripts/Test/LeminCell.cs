using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
	public class LeminCell : MonoBehaviour
	{
		private Lemin lemin;
		private Vector2 pos;

		public TMP_Text text;
		public Lemin.ECaptured type;
		public Image img;

		public void Init(Lemin lemin, Vector2 pos)
		{
			this.lemin = lemin;
			this.pos = pos;
		}

		public void OnClick_Press()
		{
			lemin.UpdateLeminCell(this);
		}
	}
}
