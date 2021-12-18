using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProxyCore.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace ProxyCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {

        [HttpPost()]
        [Route("WithProxy")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProxyResponseDto), Description = "Hidden links listed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProxyResponseDto), Description = "An error occurred")]
        public async Task<ActionResult<ProxyResponseDto>> ConnectWithProxy([FromBody] ProxyRequestInput input)
        {

            var proxyResponseDto = new ProxyResponseDto();
            try
            {

                var proxy = new WebProxy
                {

                    Address = new Uri(input.ProxyUrl),
                    Credentials = new NetworkCredential(input.ProxyUsername, input.ProxyPassword)
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                    UseProxy = true
                };

                using (var client = new HttpClient(httpClientHandler))
                {
                    var response = await client.GetAsync(input.RequestUrl);
                    if (response.IsSuccessStatusCode)
                    {

                        proxyResponseDto.ReturnStr = await response.Content.ReadAsStringAsync();
                        proxyResponseDto.IsSuccess = true;


                        return proxyResponseDto;
                    }
                    else
                    {
                        proxyResponseDto.IsSuccess = false;
                        proxyResponseDto.ErrorMessage = response.ReasonPhrase;

                        return proxyResponseDto;
                    }
                }

            }
            catch (Exception ex)
            {

                proxyResponseDto.IsSuccess = false;
                proxyResponseDto.ErrorMessage = ex.Message;

                return proxyResponseDto;
            }
        }

        [HttpPost()]
        [Route("WithoutProxy")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProxyResponseDto), Description = "Hidden links listed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProxyResponseDto), Description = "An error occurred")]
        public async Task<ActionResult<ProxyResponseDto>> ConnectWithoutProxy([FromBody] ProxyRequestInput input)
        {

            var proxyResponseDto = new ProxyResponseDto();
            try
            {
                using (var httpClient = new HttpClient(new HttpClientHandler { UseCookies = true }, true))
                {
                    var response = await httpClient.GetAsync(input.RequestUrl);
                    if (response.IsSuccessStatusCode)
                    {

                        proxyResponseDto.ReturnStr = await response.Content.ReadAsStringAsync();
                        proxyResponseDto.IsSuccess = true;


                        return proxyResponseDto;
                    }
                    else
                    {
                        proxyResponseDto.IsSuccess = false;
                        proxyResponseDto.ErrorMessage = response.ReasonPhrase;

                        return proxyResponseDto;
                    }
                }

            }
            catch (Exception ex)
            {

                proxyResponseDto.IsSuccess = false;
                proxyResponseDto.ErrorMessage = ex.Message;

                return proxyResponseDto;
            }
        }
    }
}
