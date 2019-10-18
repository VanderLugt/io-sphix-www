using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;

namespace CustomTwilioClient
{
   public class TwilioVideoService : ITwilioVideoService
    {
        private readonly TwilioVerifySettings _twilioSettings;
        public TwilioVideoService(IOptions<TwilioVerifySettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }
        public string GetTwilioJwtToken(string identity)
           => new Token(_twilioSettings.AccountSID,
                        _twilioSettings.APIKeySID,
                        _twilioSettings.APISecret,
                        identity ?? Guid.NewGuid().ToString(),
                        grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();
        /// <summary>
        /// This function will create video meeting room
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public string CreateVideoRoom(string roomName,int maxParticipants,string statusCallback="")
        {
            TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);

            var room = RoomResource.Create(
                recordParticipantsOnConnect: true,
                statusCallback: new Uri(statusCallback),
                type: RoomResource.RoomTypeEnum.Group,
                maxParticipants: maxParticipants,
                uniqueName: roomName
            );
            return room.Sid;
        }
    }
}
