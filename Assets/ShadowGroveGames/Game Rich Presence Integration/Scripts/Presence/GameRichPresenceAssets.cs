﻿using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Attributes;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence
{
	[System.Serializable]
	public class GameRichPresenceAsset
	{
		/// <summary>
		/// The key of the image to be displayed.
		/// <para>Max 32 Bytes.</para>
		/// </summary>
		[CharacterLimit(32, enforce = true)]
		[Tooltip("The key of the image to be displayed in the large square.")]
		public string image;

		/// <summary>
		/// The tooltip of the image.
		/// <para>Max 128 Bytes.</para>
		/// </summary>
		[CharacterLimit(128, enforce = true)]
		[Tooltip("The tooltip of the large image.")]
		public string tooltip;

		/// <summary>
		/// Is the asset object empty?
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(image) && string.IsNullOrEmpty(tooltip);
		}
	}
}