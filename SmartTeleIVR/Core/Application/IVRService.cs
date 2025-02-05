using SmartTeleIVR.Core.Domain;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace SmartTeleIVR.Core.Application
{
    public class IVRService : IIVRService
    {
        private readonly Dictionary<string, string> userBookings = new();

        public string GetWelcomeMessage(string callerPhoneNumber)
        {
            try
            {
                var response = new VoiceResponse();
                response.Say("Hello, and welcome to the Smart Tele System, your personal assistant for scheduling appointments.", voice: "Polly.Joanna", language: "en-US");

                var user = ValidateUser(callerPhoneNumber);
                if (user == null)
                {
                    response.Say("Sorry, we couldn't find your details. Please make sure you're using a registered phone number, or reach out to our support team for assistance.", voice: "Polly.Joanna", language: "en-US");
                    response.Hangup();
                    return response.ToString();
                }

                var gather = new Gather
                {
                    NumDigits = 1,
                    Action = new Uri("/api/ivr/menu", UriKind.Relative),
                    Method = "POST"
                };
                gather.Say($"Hi {user.Name}, press 1 to schedule a new appointment, or press 2 to check an existing booking.", voice: "Polly.Joanna", language: "en-US");
                response.Append(gather);

                return response.ToString();
            }
            catch (Exception ex)
            {
                return HandleError(callerPhoneNumber, "Error in GetWelcomeMessage", ex);
            }
        }

        public string HandleMenuOptions(string digits, string callerPhoneNumber)
        {
            var response = new VoiceResponse();
            try
            {
                var actions = new Dictionary<string, Action>
                {
                    { "1", () => ScheduleAppointment(response, callerPhoneNumber) },
                    { "2", () => CheckBooking(response, callerPhoneNumber) }
                };

                if (actions.TryGetValue(digits, out var action))
                {
                    action.Invoke();
                }
                else
                {
                    response.Say("Oops! That was an invalid selection. Please press 1 to schedule an appointment, or press 2 to check your booking.", voice: "Polly.Joanna", language: "en-US");
                    response.Redirect(new Uri("/api/ivr/menu", UriKind.Relative));
                }

                return response.ToString();
            }
            catch (Exception ex)
            {
                return HandleError(callerPhoneNumber, "Error in HandleMenuOptions", ex);
            }
        }

        private void ScheduleAppointment(VoiceResponse response, string callerPhoneNumber)
        {
            var gather = new Gather
            {
                NumDigits = 4,
                Action = new Uri("/api/ivr/book-appointment", UriKind.Relative),
                Method = "POST"
            };
            gather.Say("Please enter your preferred time slot in a 24-hour format. For example, enter one four three zero for 2:30 PM.", voice: "Polly.Joanna", language: "en-US");
            response.Append(gather);
        }

        private void CheckBooking(VoiceResponse response, string callerPhoneNumber)
        {
            var users = HardcodedData.GetUsers();
            var user = users.FirstOrDefault(u => u.PhoneNumber == callerPhoneNumber);

            if (user != null)
            {
                var appointments = HardcodedData.GetAppointments();
                var appointment = appointments.FirstOrDefault(a => a.UserID == user.UniqueID);

                if (appointment != null)
                {
                    response.Say($"Your appointment has been scheduled for {appointment.AppointmentTime:hh:mm tt}, and it is currently marked as {appointment.Status}.", voice: "Polly.Joanna", language: "en-US");
                    response.Say("Thank you for calling. For your health and safety, please make sure to follow all recommended precautions. Have a safe and healthy day!", voice: "Polly.Joanna", language: "en-US");
                    response.Hangup();
                    response.ToString();
                }
                else
                {
                    var gather = new Gather
                    {
                        NumDigits = 1,
                        Action = new Uri("/api/ivr/menu", UriKind.Relative),
                        Method = "POST"
                    };
                    response.Say("It seems you don't have an appointment yet. Press 1 to book one now.", voice: "Polly.Joanna", language: "en-US");
                    response.Append(gather);

                }
            }
            else
            {
                response.Say("Sorry, we couldn't find your number in our system. Please contact support for help.", voice: "Polly.Joanna", language: "en-US");
                response.Hangup();
                response.ToString();
            }
        }

        public string HandleBookAppointment(string digits, string callerPhoneNumber)
        {
            var response = new VoiceResponse();

            try
            {
                if (!int.TryParse(digits, out int timeSlot) || digits.Length != 4 || timeSlot < 0 || timeSlot > 2359)
                {
                    var gather = new Gather
                    {
                        NumDigits = 4,
                        Action = new Uri("/api/ivr/book-appointment", UriKind.Relative),
                        Method = "POST"
                    };
                    response.Say("Hmm, that seems to be an invalid time format. Please enter a valid 4-digit 24-hour time, like one four three zero for 2:30 PM.", voice: "Polly.Joanna", language: "en-US");
                    response.Append(gather);
                }

                var users = HardcodedData.GetUsers();
                var user = users.FirstOrDefault(u => u.PhoneNumber == callerPhoneNumber);

                if (user == null)
                {
                    response.Say("Sorry, we couldn't find your number. Please contact support for assistance.", voice: "Polly.Joanna", language: "en-US");
                    response.Hangup();
                    return response.ToString();
                }

                var now = DateTime.Now;
                var appointmentTime = new DateTime(now.Year, now.Month, now.Day, timeSlot / 100, timeSlot % 100, 0);

                userBookings[callerPhoneNumber] = appointmentTime.ToString("HH:mm");

                var appointments = HardcodedData.GetAppointments();
                appointments.Add(new AppointmentDetails
                {
                    AppointmentID = $"A{appointments.Count + 1:0000}",
                    UserID = user.UniqueID,
                    AppointmentTime = appointmentTime,
                    Status = "Confirmed"
                });

                response.Say($"Your appointment has been confirmed for {appointmentTime:hh:mm tt}. Thank you for using Smart Tele!", voice: "Polly.Joanna", language: "en-US");
            }
            catch (Exception ex)
            {
                response.Say("Oh no, something went wrong while booking your appointment. Please try again later.", voice: "Polly.Joanna", language: "en-US");
                return HandleError(callerPhoneNumber, "Error in HandleBookAppointment", ex);
            }

            return response.ToString();
        }

        private User ValidateUser(string phoneNumber)
        {
            return HardcodedData.GetUsers().FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }

        private string HandleError(string callerPhoneNumber, string context, Exception ex)
        {
            var response = new VoiceResponse();
            response.Say("We're sorry, an error occurred while processing your request. Please try again later.", voice: "Polly.Joanna", language: "en-US");
            response.Hangup();
            return response.ToString();
        }
    }
}
