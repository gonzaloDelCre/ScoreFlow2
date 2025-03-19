using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace API
{
    public class LambdaService
    {
        private readonly IAmazonLambda _lambdaClient;

        public LambdaService()
        {
            // Utilizando las credenciales predeterminadas de AWS
            var credentials = new EnvironmentVariablesAWSCredentials();
            _lambdaClient = new AmazonLambdaClient(credentials, Amazon.RegionEndpoint.EUWest3); // Cambia a tu región
        }

        public async Task<string> InvokeLambdaAsync(string functionName, string payload)
        {
            try
            {
                var request = new InvokeRequest
                {
                    FunctionName = functionName,
                    Payload = payload
                };

                var response = await _lambdaClient.InvokeAsync(request);

                if (response.StatusCode == 200)
                {
                    // Deserializa la respuesta de Lambda
                    var responseString = System.Text.Encoding.UTF8.GetString(response.Payload.ToArray());
                    var result = JsonConvert.DeserializeObject<string>(responseString);
                    return result;
                }

                return $"Error invoking Lambda: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
