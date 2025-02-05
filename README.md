# SmartTeleIVR

**SmartTeleIVR** is an Interactive Voice Response (IVR) system designed to automate telephonic interactions, allowing users to book, check, or cancel appointments via phone.

## Features

- **Automated Call Handling**: Answers inbound calls and guides users through menu options.
- **Appointment Management**: Enables users to book, check, or cancel appointments.
- **Twilio Integration**: Manages call flows and voice synthesis.

## Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Twilio Account](https://www.twilio.com/try-twilio)
- [Ngrok](https://ngrok.com/) (for local testing)

## Setup and Start

1. **Clone the Repository**

   ```bash
   git clone https://github.com/sushantSDN/SmartTeleIVR.git
   cd SmartTeleIVR
   ```

2. **Configure Application Settings** --Optional

   - Rename `appsettings.example.json` to `appsettings.json`.
   - Update the following fields with your Twilio account details:

     ```json
     {
       "Twilio": {
         "AccountSid": "your_account_sid",
         "AuthToken": "your_auth_token",
         "PhoneNumber": "your_twilio_phone_number"
       },
       "Ngrok": {
         "Url": "your_ngrok_url"
       }
     }
     ```

3. **Run the Application**

   ```bash
   dotnet run --project SmartTeleIVR.Api
   ```

4. **Expose Local Server with Ngrok**

   In a new terminal window, run:

   ```bash
   ngrok http https://localhost:5001
   ```

   Note the `Forwarding` URL provided by Ngrok (e.g., `https://abc123.ngrok.io`).

5. **Configure Twilio Webhook**

   - Log in to your [Twilio Console](https://www.twilio.com/console).
   - Navigate to **Phone Numbers** > **Manage** > **Active Numbers**.
   - Select your Twilio phone number.
   - In the **Voice & Fax** section, set the **A CALL COMES IN** field to your Ngrok URL appended with `/api/ivr/welcome` (e.g., `https://abc123.ngrok.io/api/ivr/welcome`).
   - Save the configuration.

## Project Flow

1. **Incoming Call**: A user calls the Twilio phone number.
2. **Welcome Message**: The IVR system plays a welcome message and presents menu options:
   - Press `1` to book an appointment.
   - Press `2` to check an existing appointment.
   - Press `3` to cancel an appointment.
3. **User Input**: The user selects an option by pressing the corresponding number.
4. **Process Request**: The system processes the user's request:
   - For booking, it prompts for a 4-digit time (e.g., `1430` for 2:30 PM).
   - For checking or canceling, it retrieves or updates the appointment status.
5. **Confirmation**: The system confirms the action and provides relevant information.
6. **End Call**: The system thanks the user and ends the call.

## Related References and Documentation

- **Twilio IVR Tutorial**: [Build Interactive Voice Response (IVR) Phone Tree](https://www.twilio.com/docs/voice/tutorials/build-interactive-voice-response-ivr-phone-tree/csharp#send-caller-input-to-the-intended-route)
- **Twilio IVR Phone Tree Example (C#)**: [TwilioDevEd/ivr-phone-tree-csharp GitHub Repository](https://github.com/TwilioDevEd/ivr-phone-tree-csharp.git)

## License

No license specified.
