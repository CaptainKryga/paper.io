using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
	public class LeminCell : MonoBehaviour
	{
		private Lemin lemin;

		public TMP_Text text;
		public Lemin.ECaptured type;
		public Image img;

		public void Init(Lemin lemin, Vector2 pos)
		{
			this.lemin = lemin;
		}

		public void OnClick_Press()
		{
			lemin.UpdateLeminCell(this);
		}
	}
}
