namespace ConfigurationAPI.Helpers
{
    using global::ConfigurationAPI.Models;
    using System;

    namespace ConfigurationAPI.Helpers
    {
        public static class ResponseHelper
        {
            public static Response<object> CreateResponse(string type, int statusCode, string message, string value)
            {
                switch (type.ToLower())
                {
                    case "int":
                    case "system.int32":
                        if (int.TryParse(value, out int intValue))
                        {
                            return ConvertResponse(new Response<int> { StatusCode = statusCode, Message = message, Data = intValue });
                        }
                        break;
                    case "bool":
                    case "system.boolean":
                        if (bool.TryParse(value, out bool boolValue))
                        {
                            return ConvertResponse(new Response<bool> { StatusCode = statusCode, Message = message, Data = boolValue });
                        }
                        break;
                    case "float":
                    case "system.single":
                        if (float.TryParse(value, out float floatValue))
                        {
                            return ConvertResponse(new Response<float> { StatusCode = statusCode, Message = message, Data = floatValue });
                        }
                        break;
                    case "double":
                    case "system.double":
                        if (double.TryParse(value, out double doubleValue))
                        {
                            return ConvertResponse(new Response<double> { StatusCode = statusCode, Message = message, Data = doubleValue });
                        }
                        break;
                    case "string":
                    case "system.string":
                        return ConvertResponse(new Response<string> { StatusCode = statusCode, Message = message, Data = value });
                    default:
                        return new Response<object> { StatusCode = 400, Message = "Unsupported data type.", Data = null };
                }
                return new Response<object> { StatusCode = 400, Message = "Invalid value for the specified data type.", Data = null };
            }

            private static Response<object> ConvertResponse<T>(Response<T> response)
            {
                return new Response<object>
                {
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                    Data = response.Data
                };
            }
        }
    }

}
