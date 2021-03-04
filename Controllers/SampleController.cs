using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebhooksReceiver.Models;

namespace WebhooksReceiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private AuthProfile _authProfile;

        public SampleController()
        {
            _authProfile = new AuthProfile();
            _authProfile.ApiUrl = @"https://institution-sim.clearbank.co.uk/";
            _authProfile.ApiToken = @"";

            _authProfile.ClientPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----

-----END RSA PRIVATE KEY-----";

            _authProfile.ClearBankPublicKey = @"-----BEGIN PUBLIC KEY-----

-----END PUBLIC KEY-----";
        }

        [HttpPost]
        [Route("api")]
        public async Task<ActionResult<string>> Call([FromBody] ApiRequest request)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_authProfile.ApiUrl);

            var req = new HttpRequestMessage(HttpMethod.Post, "v1/test");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authProfile.ApiToken);
            req.Content = new StringContent(request.Body, Encoding.UTF8, "application/json");
            req.Headers.Add("DigitalSignature", DigitalSignature.Generate(request.Body, _authProfile.ClientPrivateKey));
            req.Headers.Add("X-Request-Id", Guid.NewGuid().ToString("N"));

            var response = await client.SendAsync(req);

            var body = await response.Content.ReadAsStringAsync();
            response.Headers.TryGetValues("X-Correlation-Id", out var headers);
            var correlationId = headers.First();

            return $"Body: {body}, correlation id: {correlationId}";
        }

        [HttpPost]
        [Route("webhook")]
        public IActionResult Post([FromBody] WebhookRequest1 webhookRequest1, [FromHeader(Name = "DigitalSignature")] string digitalSignature)
        {
            Console.WriteLine($"Received webhook {webhookRequest1.Type}");
            string body;

            // We need to read the whole body here again to calculate hash, because we can't be certain that conversion back from WebhookRequest1 object will result in the same string
            Request.Body.Position = 0;
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }

            var verified = DigitalSignature.Verify(digitalSignature, body, _authProfile.ClearBankPublicKey);

            if (!verified)
            {
                return BadRequest("Incorrect signature");
            }

            // In production system you should put that webhook into the internal queue for processing 
            // Don't do heave processing here and send response as quick as possible

            var result = new WebhookResponse1 { Nonce = webhookRequest1.Nonce };

            var response = JsonSerializer.Serialize(result);
            var signature = DigitalSignature.Generate(response, _authProfile.ClientPrivateKey);

            Request.HttpContext.Response.Headers.Add("DigitalSignature", signature);

            return Content(response);
        }
    }
}
