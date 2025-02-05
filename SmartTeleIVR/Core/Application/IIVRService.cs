namespace SmartTeleIVR.Core.Application
{
    public interface IIVRService
    {
        string GetWelcomeMessage(string callerPhoneNumber);
        string HandleMenuOptions(string digits, string callerPhoneNumber);
        string HandleBookAppointment(string digits, string callerPhoneNumber);
    }
}
