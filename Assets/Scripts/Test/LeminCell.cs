using UnityEngine;

namespace Test
{
	public class LeminCell
	{
		public Lemin.ECaptured type;

		private TMPro.TMP_Text debug;

		public string Debug
		{
			get => debug.text;
			set
			{
				if (!debug)
					return;

				debug.text = value;
			}
		}

		public LeminCell(TMPro.TMP_Text debug)
		{
			this.debug = debug;
			this.debug.text = "-";
		}
		
		public LeminCell()
		{
			
		}
	}
}