namespace CustomTwilioClient
{
public  interface ITwilioVideoService
    {
        string GetTwilioJwtToken(string identity);
        string CreateVideoRoom(string roomName,int maxParticipants,string statusCallback="");
    }
}
