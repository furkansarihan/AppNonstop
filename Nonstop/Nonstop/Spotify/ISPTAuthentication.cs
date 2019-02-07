using System;

namespace Nonstop.Forms.Spotify
{
    public interface ISPTTokenReceiver
    {
        void onTokenReady(object sender, TokenReceiverEventArgs tokenArgs);
    }

    public class TokenReceiverEventArgs : EventArgs
    {
        public string token { get; set; }
    }

    public interface ISPTAuthentication
    {
        // https://stackoverflow.com/a/937219/
        event EventHandler<TokenReceiverEventArgs> tokenReady;
        void registerToken(ISPTTokenReceiver receiver);
        void authRequest(string[] scopes);
    }
}
