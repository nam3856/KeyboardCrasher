using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence
{
	[System.Serializable]
	public class GameRichPresenceButton
	{
		/// <summary>
		/// The text on the button to be displayed
		/// </summary>
		[Tooltip("The label on the button to be displayed")]
		public string label;

		/// <summary>
		/// The tooltip of the image.
		/// </summary>]
		[Tooltip("The URL on the button to be displayed")]
		public string url;

		/// <summary>
		/// Is the asset object empty?
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(label) && string.IsNullOrEmpty(url);
		}
	}
}