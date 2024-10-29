
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OriginalSDK
{
  public class ErrorDetail
  {
    public required string Message { get; set; }
    public required string Code { get; set; }
    public string? FieldName { get; set; }
  }

  public class Error
  {
    public required string Type { get; set; }

    [JsonConverter(typeof(ErrorDetailConverter))]
    public required object Detail { get; set; }

    public Error()
    {
      Type = string.Empty;
    }

    public Error(ErrorDetail detail)
    {
      Detail = detail;
    }

    public Error(List<ErrorDetail> details)
    {
      Detail = details;
    }

    public bool IsSingleDetail => Detail is ErrorDetail;
    public bool IsDetailList => Detail is List<ErrorDetail>;

    public ErrorDetail? GetSingleDetail() => Detail as ErrorDetail;
    public List<ErrorDetail>? GetDetailList() => Detail as List<ErrorDetail>;

    public string GetMessage()
    {
      if (IsSingleDetail)
        return GetSingleDetail()?.Message ?? string.Empty;
      else if (IsDetailList && GetDetailList()?.Count > 0)
        return GetDetailList()?[0].Message ?? string.Empty;

      return string.Empty;
    }
  }

  public class ErrorDetailConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(object);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value is ErrorDetail detail)
      {
        serializer.Serialize(writer, detail);
      }
      else if (value is List<ErrorDetail> detailList)
      {
        serializer.Serialize(writer, detailList);
      }
      else
      {
        writer.WriteNull();
      }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      var token = JToken.Load(reader);
      if (token.Type == JTokenType.Object)
      {
        return token.ToObject<ErrorDetail>(serializer);
      }
      else if (token.Type == JTokenType.Array)
      {
        return token.ToObject<List<ErrorDetail>>(serializer) ?? new List<ErrorDetail>();
      }
      throw new JsonSerializationException("Unexpected JSON format for ErrorDetail.");
    }
  }

  public class OriginalErrorData
  {
    public bool Success { get; set; } = false;
    public required Error Error { get; set; }

  }

  public class OriginalErrorCode
  {
    public const string ClientError = "client_error";
    public const string ServerError = "server_error";
    public const string ValidationError = "validation_error";
  }

  public class OriginalError : Exception
  {
    new public string Message { get; private set; }
    public int Status { get; private set; }
    new public object Data { get; private set; }
    public string Code { get; private set; }

    public OriginalError(string message, int status, OriginalErrorData data, string originalErrorCode)
        : base(message)
    {
      Message = message;
      Status = status;
      Data = data;
      Code = originalErrorCode;
    }

    public override string ToString() => $"{Message} - {Status} - {Code} - {Data}";
  }

  public class ClientException : OriginalError
  {
    public ClientException(string message, int status, OriginalErrorData data)
        : base(message, status, data, OriginalErrorCode.ClientError) { }
  }

  public class ServerException : OriginalError
  {
    public ServerException(string message, int status, OriginalErrorData data)
        : base(message, status, data, OriginalErrorCode.ServerError) { }
  }

  public class ValidationException : OriginalError
  {
    public ValidationException(string message, int status, OriginalErrorData data)
        : base(message, status, data, OriginalErrorCode.ValidationError) { }
  }

  public static class ErrorUtils
  {
    public static void ParseAndRaiseError(OriginalErrorData? parsedResult, string reason, int status)
    {
      var result = parsedResult;
      var error = result?.Error;

      if (error != null && result != null)
      {
        var errorType = error.Type;
        var detail = error.Detail is List<ErrorDetail> detailList ? detailList[0] : (ErrorDetail)error.Detail;

        var message = detail?.Message ?? reason;

        if (errorType == OriginalErrorCode.ServerError)
        {
          throw new ServerException(message, status, result);
        }
        else if (errorType == OriginalErrorCode.ValidationError)
        {
          throw new ValidationException(message, status, result);
        }
        else
        {
          throw new ClientException(message, status, result);
        }
      }
      else
      {
        throw new ClientException(
          reason,
          status,
          new OriginalErrorData
          {
            Error = new Error
            {
              Type = OriginalErrorCode.ClientError,
              Detail = new ErrorDetail
              {
                Message = "A client error occurred",
                Code = "client_error",
                FieldName = null
              }
            }
          }
        );
      }
    }
  }
}
