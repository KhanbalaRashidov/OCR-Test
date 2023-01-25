using System;
using Telesign;


    string customerId = "A1F6A5EA-9882-4889-9F04-A0777235B76E";
    string apiKey = "ZUuhNMYPb3GHiXnIQLpAgBrBTk+McLShIIQxOt6WXRs8XKkp1/vVAAX+72vnEwsxwGjLtGaZy4Huk6GytRiCYw==";

    string phoneNumber = "905375739578";

    string verifyCode = "12345";
    string message = string.Format("Your code is {0}", verifyCode);
    string messageType = "OTP";

    try
    {
        MessagingClient messagingClient = new MessagingClient(customerId, apiKey);
        RestClient.TelesignResponse telesignResponse = messagingClient.Message(phoneNumber, message, messageType);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }

    Console.WriteLine("Press any key to quit.");
    Console.ReadKey();
        