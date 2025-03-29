using DiscordRPC;
using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Control;
using ShadowGroveGames.GameRichPresenceIntegration.Scripts.Presence;
using UnityEngine;

namespace ShadowGroveGames.GameRichPresenceIntegration.Scripts
{
    public class GameRichPresenceManagerScript : MonoBehaviour
    {
        public const string EXAMPLE_DISCORD_APPLICATION = "1046051140657221712";

        [Header("Application")]
        [Tooltip("Your Discord Application ID. To get a discord application id open the Discord Developr Portal and click on your Application.")]
        public string DiscordApplicationId = EXAMPLE_DISCORD_APPLICATION;

        [Tooltip("Check before initialize Discord Rich Precense if discord is open.")]
        public bool CheckDiscordIsOpen = true;

        public static GameRichPresenceManagerScript Instance { get; private set; }

        public DiscordRpcClient Client { get; private set; } = null;

        private UnityNamedPipe _unityNamedPipe = null;

        public bool IsInitialized => Client != null && Client.IsInitialized;

        [SerializeField]
        private GameRichPresencePresence _richPresence;
        public GameRichPresencePresence RichPresence { get { return _richPresence; } }

        [Header("Events")]
        public GameRichPresenceEvents events = new GameRichPresenceEvents();

        [Header("Debug")]
        [Tooltip("Discord IPC logging level.")]
        public DiscordRPC.Logging.LogLevel logLevel = DiscordRPC.Logging.LogLevel.Warning;

        public GameRichPresenceUser CurrentUser { get { return _currentUser; } }
        [Tooltip("The current Discord user. This does not get set until the first Ready event.")]

        [SerializeField]
        private GameRichPresenceUser _currentUser;

        private bool _startUpDone = false;
        private float _discordIsOpenCheckInverval = 2f;
        private float _discordIsOpenCheckThreshold = 0f;

        void OnEnable()
        {
            if (_startUpDone && !IsInitialized)
                Initialize();
        }

        void OnDisable()
        {
            Shutdown();
        }

        void OnDestroy()
        {
            Shutdown();
        }

        void Start()
        {
            if (!Instance)
            {
                if (Client != null)
                {
                    if (logLevel <= DiscordRPC.Logging.LogLevel.Warning)
                        Debug.LogWarning("Client already exists! Disposing Early.");

                    Shutdown();
                }

                Initialize();
                _startUpDone = true;
            }
        }

        void FixedUpdate()
        {
            DiscordIsOpenCheck();

            if (Client == null)
                return;

            Client.Logger.Level = logLevel;
            Client.Invoke();
        }

        public void Initialize()
        {
            if (!ValidateStartCondtions())
                return;

            // Assign the instance
            Instance = this;
            DontDestroyOnLoad(this);

            if (CheckDiscordIsOpen && !GameRichPresenceStatus.IsActive())
            {
                Debug.Log("Discord is not open. Please open Discord to use Rich Presence!");
                return;
            }

            StartDiscordClient();
        }

        private void DiscordIsOpenCheck()
        {
            if (!CheckDiscordIsOpen || !gameObject.activeSelf)
                return;

            if (_discordIsOpenCheckThreshold > 0)
            {
                _discordIsOpenCheckThreshold -= Time.deltaTime;
                return;
            }

            _discordIsOpenCheckThreshold = _discordIsOpenCheckInverval;

            if (logLevel <= DiscordRPC.Logging.LogLevel.Trace)
                Debug.Log("Recheck if Discord Client is open...");

            bool discordIsActive = GameRichPresenceStatus.IsActive();

            if (discordIsActive && !IsInitialized)
            {
                StartDiscordClient();
                return;
            }

            if (!discordIsActive && IsInitialized)
            {
                Shutdown();
                return;
            }
        }

        private void StartDiscordClient()
        {
            DiscordRPC.Logging.ILogger logger = InitializeLogger();

            _unityNamedPipe = new UnityNamedPipe();
            Client = new DiscordRpcClient(DiscordApplicationId, -1, logger, false, _unityNamedPipe);

            SubscribeEvents();

            // Start the Client
            Client.Initialize();

            // Enqueue initial presence update
            SetPresence(_richPresence);

            if (logLevel <= DiscordRPC.Logging.LogLevel.Info)
                Debug.Log("[Discord Rich Presence] Discord Rich Presence intialized and connecting...");
        }

        public void Shutdown()
        {
            _currentUser = null;
            if (logLevel <= DiscordRPC.Logging.LogLevel.Info)
                Debug.Log("[Discord Rich Presence] Disposing Discord IPC Client...");


            if (Client != null)
            {
                Client.ClearPresence();
                Client.Deinitialize();
                Client.Dispose();
                Client = null;
            }

            Instance = null;
            Debug.Log("[Discord Rich Presence] Finished disconnecting");
        }

        public void SetPresence(GameRichPresencePresence presence)
        {
            if (Client == null)
            {
                Debug.LogError("[Discord Rich Presence] Attempted to send a presence update but no Client exists!");
                return;
            }

            _richPresence = presence;
            Client.SetPresence(presence != null ? presence.ToRichPresence() : null);
        }

        private bool ValidateStartCondtions()
        {
            // Check if instance is already exists and Destory double instances
            if (Instance != null && Instance != this)
            {
                if (logLevel <= DiscordRPC.Logging.LogLevel.Warning)
                    Debug.LogWarning("[Discord Rich Presence] Discord Manager exist already. Destroying self.", Instance);

                Destroy(this);
                return false;
            }

            // Check if client already exit
            if (Client != null)
            {
                if (logLevel <= DiscordRPC.Logging.LogLevel.Error)
                    Debug.LogError("[Discord Rich Presence] Cannot initialize a new Client when one is already initialized.");

                return false;
            }

            return true;
        }

        private DiscordRPC.Logging.ILogger InitializeLogger()
        {
            DiscordRPC.Logging.ILogger logger = null;

            if (Debug.isDebugBuild)
                logger = new DiscordRPC.Logging.FileLogger("discordrpc.log") { Level = logLevel };

            if (Application.isEditor)
                logger = new UnityLogger() { Level = logLevel };

            //We are starting the Client. Below is a break down of the parameters.
            if (logLevel <= DiscordRPC.Logging.LogLevel.Info)
                Debug.Log("[Discord Rich Presence] Starting Discord Rich Presence");

            return logger;
        }

        private void SubscribeEvents()
        {
            Client.OnError += (s, args) => Debug.LogError("[Discord Rich Presence] Error Occured within the Discord IPC: (" + args.Code + ") " + args.Message);
            Client.OnReady += (s, args) =>
            {
                Debug.Log("[Discord Rich Presence] Connection established and received READY from Discord IPC. Sending our previous Rich Presence and Subscription.");

                _currentUser = args.User;
                _currentUser.GetAvatar(this, DiscordAvatarSize.x128);
            };

            Client.OnPresenceUpdate += (s, args) =>
            {
                if (logLevel <= DiscordRPC.Logging.LogLevel.Info)
                {
                    Debug.Log("[Discord Rich Presence] Our Rich Presence has been updated. Applied changes to local store.");
                    Debug.Log(args.Presence.State);
                }

                _richPresence = (GameRichPresencePresence)args.Presence;
            };

            // Register unity events
            events.RegisterEvents(Client);
        }

        public GameRichPresencePresence UpdateDetails(string details)
        {
            if (Client == null)
                return null;

            _richPresence.details = details;

            return (GameRichPresencePresence)Client.UpdateDetails(details);
        }

        public GameRichPresencePresence UpdateState(string state)
        {
            if (Client == null)
                return null;

            _richPresence.state = state;

            return (GameRichPresencePresence)Client.UpdateState(state);
        }

        public GameRichPresencePresence UpdateLargeAsset(GameRichPresenceAsset asset)
        {
            if (Client == null)
                return null;

            if (asset == null)
                return (GameRichPresencePresence)Client.UpdateLargeAsset("", "");

            _richPresence.largeAsset = asset;

            return (GameRichPresencePresence)Client.UpdateLargeAsset(asset.image, asset.tooltip);
        }
        public GameRichPresencePresence UpdateSmallAsset(GameRichPresenceAsset asset)
        {
            if (Client == null)
                return null;

            if (asset == null)
                return (GameRichPresencePresence)Client.UpdateSmallAsset("", "");

            _richPresence.smallAsset = asset;

            return (GameRichPresencePresence)Client.UpdateSmallAsset(asset.image, asset.tooltip);
        }

        public GameRichPresencePresence UpdateStartTime()
        {
            if (Client == null)
                return null;

            _richPresence.startTime = new GameRichPresenceTimestamp();

            return (GameRichPresencePresence)Client.UpdateStartTime();
        }

        public GameRichPresencePresence UpdateStartTime(GameRichPresenceTimestamp timestamp)
        {
            if (Client == null)
                return null;

            _richPresence.startTime = timestamp;

            return (GameRichPresencePresence)Client.UpdateStartTime(timestamp.GetDateTime());
        }

        public GameRichPresencePresence UpdateEndTime()
        {
            if (Client == null)
                return null;

            _richPresence.endTime = new GameRichPresenceTimestamp();

            return (GameRichPresencePresence)Client.UpdateEndTime();
        }

        public GameRichPresencePresence UpdateEndTime(GameRichPresenceTimestamp timestamp)
        {
            if (Client == null)
                return null;

            _richPresence.endTime = timestamp;

            return (GameRichPresencePresence)Client.UpdateEndTime(timestamp.GetDateTime());
        }

        public GameRichPresencePresence UpdateClearTime()
        {
            if (Client == null)
                return null;

            _richPresence.startTime = GameRichPresenceTimestamp.Invalid;
            _richPresence.endTime = GameRichPresenceTimestamp.Invalid;

            return (GameRichPresencePresence)Client.UpdateClearTime();
        }
    }

}