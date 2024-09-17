using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public static class HashHelper
{
    public static string HashCustomerInfo(object customerInfo)
    {
        // Serialize the customer information to a JSON string
        var jsonString = JsonConvert.SerializeObject(customerInfo);

        // Compute the hash of the JSON string
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(jsonString));
            return Convert.ToBase64String(hashBytes);
        }
    }
}