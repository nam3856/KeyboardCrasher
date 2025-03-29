using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Attributes;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence
{
    [System.Serializable]
    public class GameRichPresencePresence
    {
        [Header("Basic Details")]

        /// <summary>
        /// The details about the game. Appears underneath the game name
        /// </summary>
        [CharacterLimit(128)]
        [Tooltip("The details about the game")]
        public string details = "Playing a game";

        /// <summary>
        /// The current state of the game (In Game, In Menu etc). Appears next to the party size
        /// </summary>
        [CharacterLimit(128)]
        [Tooltip("The current state of the game (In Game, In Menu). It appears next to the party size.")]
        public string state = "In Game";

        [Header("Time Details")]

        /// <summary>
        /// The time the game started. 0 if the game hasn't started
        /// </summary>
        [Tooltip("The time the game started. Leave as 0 if the game has not yet started.")]
        public GameRichPresenceTimestamp startTime = 0;

        /// <summary>
        /// The time the game will end in. 0 to ignore endtime.
        /// </summary>
        [Tooltip("Time the game will end. Leave as 0 to ignore it.")]
        public GameRichPresenceTimestamp endTime = 0;

        [Header("Presentation Details")]

        /// <summary>
        /// The images used for the presence.
        /// </summary>
        [Tooltip("The images used for the presence")]
        public GameRichPresenceAsset largeAsset = null;
        public GameRichPresenceAsset smallAsset = null;

        [Header("Button Details (Discord can only show two Buttons)")]

        /// <summary>
        /// The buttons used for the presence. 
        /// Discord can display a maximum of two buttons. The displayed buttons can only be used by others.
        /// </summary>
        [Tooltip("The buttons used for the presence")]
        public GameRichPresenceButton[] buttons = new GameRichPresenceButton[0];

        /// <summary>
        /// Creates a new Presence object
        /// </summary>
        public GameRichPresencePresence() { }

        /// <summary>
        /// Creats a new Presence object, copying values of the Rich Presence
        /// </summary>
        /// <param name="presence">The rich presence, often received by discord.</param>
        public GameRichPresencePresence(DiscordRPC.RichPresence presence)
        {
            if (presence != null)
            {
                this.state = presence.State;
                this.details = presence.Details;

                if (presence.HasAssets())
                {
                    this.smallAsset = new GameRichPresenceAsset()
                    {
                        image = presence.Assets.SmallImageKey,
                        tooltip = presence.Assets.SmallImageText,
                    };


                    this.largeAsset = new GameRichPresenceAsset()
                    {
                        image = presence.Assets.LargeImageKey,
                        tooltip = presence.Assets.LargeImageText,
                    };
                }
                else
                {
                    this.smallAsset = new GameRichPresenceAsset();
                    this.largeAsset = new GameRichPresenceAsset();
                }

                if (presence.HasButtons())
                {
                    this.buttons = new GameRichPresenceButton[presence.Buttons.Length];

                    for (int i = 0; i < presence.Buttons.Length; i++)
                    {
                        this.buttons[i] = new GameRichPresenceButton()
                        {
                            label = presence.Buttons[i].Label,
                            url = presence.Buttons[i].Url
                        };
                    }
                }
                else
                {
                    this.buttons = new GameRichPresenceButton[0];
                }


                if (presence.HasTimestamps())
                {
                    //This could probably be made simpler
                    this.startTime = presence.Timestamps.Start.HasValue ? new GameRichPresenceTimestamp((long)presence.Timestamps.StartUnixMilliseconds.Value) : GameRichPresenceTimestamp.Invalid;
                    this.endTime = presence.Timestamps.End.HasValue ? new GameRichPresenceTimestamp((long)presence.Timestamps.EndUnixMilliseconds.Value) : GameRichPresenceTimestamp.Invalid;
                }
            }
            else
            {
                this.state = "";
                this.details = "";
                this.smallAsset = new GameRichPresenceAsset();
                this.largeAsset = new GameRichPresenceAsset();
                this.buttons = new GameRichPresenceButton[0];
                this.startTime = GameRichPresenceTimestamp.Invalid;
                this.endTime = GameRichPresenceTimestamp.Invalid;
            }
        }

        /// <summary>
        /// Converts this object into a new instance of a rich presence, ready to be sent to the discord Client.
        /// </summary>
        /// <returns>A new instance of a rich presence, ready to be sent to the discord Client.</returns>
        public DiscordRPC.RichPresence ToRichPresence()
        {
            var presence = new DiscordRPC.RichPresence();
            presence.State = this.state;
            presence.Details = this.details;

            presence.Party = null;
            presence.Secrets = null;

            if ((smallAsset != null && !smallAsset.IsEmpty()) || (largeAsset != null && !largeAsset.IsEmpty()))
            {
                presence.Assets = new DiscordRPC.Assets()
                {
                    SmallImageKey = smallAsset.image,
                    SmallImageText = smallAsset.tooltip,

                    LargeImageKey = largeAsset.image,
                    LargeImageText = largeAsset.tooltip
                };
            }

            if (startTime.IsValid() || endTime.IsValid())
            {
                presence.Timestamps = new DiscordRPC.Timestamps();
                if (startTime.IsValid()) presence.Timestamps.Start = startTime.GetDateTime();
                if (endTime.IsValid()) presence.Timestamps.End = endTime.GetDateTime();
            }

            if (buttons.Length > 0)
            {
                presence.Buttons = new DiscordRPC.Button[buttons.Length];

                for (int i = 0; i < buttons.Length; i++)
                {
                    presence.Buttons[i] = new DiscordRPC.Button
                    {
                        Label = buttons[i].label,
                        Url = buttons[i].url
                    };
                }
            }

            return presence;
        }

        public static explicit operator DiscordRPC.RichPresence(GameRichPresencePresence presence)
        {
            return presence.ToRichPresence();
        }

        public static explicit operator GameRichPresencePresence(DiscordRPC.RichPresence presence)
        {
            return new GameRichPresencePresence(presence);
        }
    }
}
