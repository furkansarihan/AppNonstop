using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Nonstop.Forms.Spotify
{
    public class ISPTGateway : ISPTTokenReceiver, ISPTConnectionEventReceiver 
    {
        ISPTCommunicator communicator = DependencyService.Get<ISPTCommunicator>();
        ISPTAuthentication authenticator = DependencyService.Get<ISPTAuthentication>();

        public void onResult(object sender, SPTGatewayEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void onTokenReady(object sender, TokenReceiverEventArgs tokenArgs)
        {
            throw new NotImplementedException();
        }
    }
}
