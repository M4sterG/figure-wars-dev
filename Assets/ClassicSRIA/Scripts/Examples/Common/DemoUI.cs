using UnityEngine;
using UnityEngine.UI;

namespace frame8.ScrollRectItemsAdapter.Classic.Examples.Common
{
    public class DemoUI : MonoBehaviour
	{
		public Button setCountButton;
		public Text countText;
		public Button scrollToButton;
		public Text scrollToText;
		public Button addOneTailButton, removeOneTailButton, addOneHeadButton, removeOneHeadButton;
		public Toggle freezeContentEndEdge;

		public int SetCountValue { get { return 1000; } }
		public int ScrollToValue { get { return 0; } }
	}	
}
