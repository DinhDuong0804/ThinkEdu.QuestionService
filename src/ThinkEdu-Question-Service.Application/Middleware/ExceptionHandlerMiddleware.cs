using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Application.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private Task ConvertException(HttpContext context, Exception exception)
        {
            int httpStatusCode;
            var result = exception.Message;
            var response = new BaseResponse<string>
            {
                Success = false
            };

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    response.ResultCode = HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    response.ValidationErrors = validationException.ValidationErrors;
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    response.ResultCode = HttpStatusCode.BadRequest;
                    response.Message = badRequestException.Message;
                    break;
                case NotFoundException _:
                    httpStatusCode = (int)HttpStatusCode.NotFound;
                    response.ResultCode = HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    break;
                case ApiException apiException:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ResultCode = HttpStatusCode.InternalServerError;
                    response.Message = apiException.Message;
                    break;
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ResultCode = HttpStatusCode.InternalServerError;
                    response.Message = exception.Message;
                    break;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpStatusCode;

            if (result == string.Empty)
            {
                response.Message = exception.Message;
            }

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    Formatting = Formatting.None
                };

                return settings;
            };
            result = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(result);
        }
    }
}