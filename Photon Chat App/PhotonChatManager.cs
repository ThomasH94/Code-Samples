using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ChatiChat.Managers
{
    /// <summary>
    /// The purpose of this class is to manage the interactions by sending messages via the Photon Network
    /// This class will control enabling/disabling the chat room by verifying user login credentials
    /// before attempting to login a user to the Photon Network
    /// </summary>
    public class PhotonChatManager : MonoBehaviour, IChatClientListener
    {
        private ChatClient _chatClient;
        [SerializeField] private string username = null;
        private bool _channelConnected = false;

        #region Messaging
        [SerializeField] private TMP_InputField chatTMPField = null;
        [SerializeField] private TMP_Text chatDisplayTMP = null;
        private string _privateRecipient = "";
        private string _currentMessage;
        #endregion

        // TODO: Move the Join Chat section to a separate class and communicate with the Chat Manager to verify users
        #region UI
        [SerializeField] private TMP_InputField usernameInput = null;
        [SerializeField] private GameObject loginContainer = null;
        [SerializeField] private GameObject chatRoom = null;
        #endregion

        private void Update()
        {
            if(_channelConnected)
                _chatClient.Service();

            ValidateMessage();
        }

        #region Setup
        public void ChatConnectOnClick()
        {
            // Set our userID to the username from the username input field
            if (!string.IsNullOrEmpty(usernameInput.text))    // This is how we check input fields for null
            {
                username = usernameInput.text;
                
                _chatClient = new ChatClient(this);
                _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
                    new AuthenticationValues(username));
                
                Debug.Log("Connecting...");

                OnConnected();
            }
            else
            {
                Debug.LogError($"<color=red>ERROR!</color> Username: {usernameInput.text} is invalid!");
            }
        }
        #endregion

        #region Messaging
        public void OnUsernameValueChanged(string valueIn)
        {
            username = valueIn;
        }
        
        public void OnChatValueChanged(string message)
        {
            _currentMessage = message;
        }

        public void OnRecipientValueChanged(string recipient)
        {
            _privateRecipient = recipient;
        }
        
        private void ValidateMessage()
        {
            //Verify chat messages aren't blank before sending
            if (chatTMPField.text != "" && Input.GetKeyDown(KeyCode.Return))
            {
                OnSubmitPublicMessage();
                OnSubmitPrivateMessage();
            }
        }
        
        public void OnSubmitPublicMessage()
        {
            if (_privateRecipient == "")
            {
                // TODO: Create a channel properly..see OnConnected for an example
                _chatClient.PublishMessage("RegionChannel", _currentMessage);
                chatTMPField.text = "";
                _currentMessage = "";
            }
        }

        public void OnSubmitPrivateMessage()
        {
            // TODO: Create one SubmitMessage() method or clean up the current SubmitMessage() methods and
            // verify that the recipient exists
            if (_privateRecipient != "")
            {
                _chatClient.SendPrivateMessage(_privateRecipient, _currentMessage);
                chatTMPField.text = "";
                _currentMessage = "";
            }
        }
        #endregion

        #region IChatClientListener Methods
        public void DebugReturn(DebugLevel level, string message)
        {

        }

        public void OnDisconnected()
        {

        }

        public void OnConnected()
        {
            _channelConnected = true;
            
            string[] channels = new[] {"RegionChannel"};
            _chatClient.Subscribe(channels);
            
            loginContainer.SetActive(false);
            
            Debug.Log("Connected!");
        }

        public void OnChatStateChange(ChatState state)
        {

        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            //Get a users message
            string messagesToDisplay = "";
            for (int i = 0; i < senders.Length; i++)
            {
                // Display message as: Username: "Message"
                messagesToDisplay = $"{senders[i]}: {messages[i]}";
                chatDisplayTMP.text += "\n " + messagesToDisplay;
                
                Debug.Log(messagesToDisplay);
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            string messagesToDisplay = "";

            messagesToDisplay = $"(Private) {sender}: {message}";
            chatDisplayTMP.text += "\n " + messagesToDisplay;
                
            Debug.Log(messagesToDisplay);
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            chatRoom.SetActive(true);
            // TODO: Let's create a public message from the system like "{x} joined the chat room!"
        }

        public void OnUnsubscribed(string[] channels)
        {

        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {

        }

        public void OnUserSubscribed(string channel, string user)
        {

        }

        public void OnUserUnsubscribed(string channel, string user)
        {

        }

        #endregion
    }
}