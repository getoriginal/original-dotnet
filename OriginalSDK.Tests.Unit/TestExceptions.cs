using Newtonsoft.Json;

namespace OriginalSDK.Tests.Unit
{
  public class ExceptionTests
  {
    [Fact]
    public void ErrorDetail_Serialization_Deserialization_Works()
    {
      var detail = new ErrorDetail
      {
        Message = "A detailed message",
        Code = "error_code",
        FieldName = "field_name"
      };

      var json = JsonConvert.SerializeObject(detail);
      var deserializedDetail = JsonConvert.DeserializeObject<ErrorDetail>(json);

      Assert.Equal("A detailed message", deserializedDetail?.Message);
      Assert.Equal("error_code", deserializedDetail?.Code);
      Assert.Equal("field_name", deserializedDetail?.FieldName);
    }

    [Fact]
    public void Error_WithSingleErrorDetail_SerializesCorrectly()
    {
      var error = new
      {
        Detail = new ErrorDetail
        {
          Message = "Single error detail",
          Code = "single_code",
          FieldName = "single_field"
        },
        Type = OriginalErrorCode.ClientError
      };

      var json = JsonConvert.SerializeObject(error);
      var deserializedError = JsonConvert.DeserializeObject<Error>(json);

      Assert.Equal(OriginalErrorCode.ClientError, deserializedError?.Type);
      Assert.IsType<ErrorDetail>(deserializedError?.Detail);
      Assert.Equal("Single error detail", ((ErrorDetail)deserializedError.Detail).Message);
    }

    [Fact]
    public void Error_WithMultipleErrorDetails_SerializesCorrectly()
    {
      var errorDetails = new List<ErrorDetail>
            {
                new() { Message = "First detail", Code = "code1" },
                new() { Message = "Second detail", Code = "code2" }
            };
      var error = new Error { Detail = errorDetails, Type = OriginalErrorCode.ValidationError };

      var json = JsonConvert.SerializeObject(error);
      var deserializedError = JsonConvert.DeserializeObject<Error>(json);

      Assert.Equal(OriginalErrorCode.ValidationError, deserializedError?.Type);
      Assert.IsType<List<ErrorDetail>>(deserializedError?.Detail);
      Assert.Equal("First detail", ((List<ErrorDetail>)deserializedError.Detail)[0].Message);
    }

    [Fact]
    public void OriginalErrorData_WithSingleDetail_CreatesCorrectly()
    {
      var originalErrorData = new OriginalErrorData
      {
        Success = false,
        Error = new Error
        {
          Detail = new ErrorDetail
          {
            Message = "Single error occurred",
            Code = "error_code"
          },
          Type = OriginalErrorCode.ClientError
        }
      };

      var json = JsonConvert.SerializeObject(originalErrorData);
      var deserializedData = JsonConvert.DeserializeObject<OriginalErrorData>(json);

      Assert.False(deserializedData?.Success);
      Assert.Equal("Single error occurred", ((ErrorDetail)deserializedData!.Error.Detail).Message);
    }

    [Fact]
    public void ParseAndRaiseError_RaisesClientException_ForClientError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Success = false,
        Error = new Error
        {
          Detail = new ErrorDetail
          {
            Message = "Client error occurred",
            Code = "client_error_code"
          },
          Type = OriginalErrorCode.ClientError
        }
      };

      var ex = Assert.Throws<ClientException>(() => ErrorUtils.ParseAndRaiseError(originalErrorData, "Error reason", 400));
      Assert.Equal("Client error occurred", ex.Message);
      Assert.Equal(400, ex.Status);
    }

    [Fact]
    public void ParseAndRaiseError_RaisesServerException_ForServerError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Success = false,
        Error = new Error
        {
          Detail = new ErrorDetail
          {
            Message = "Server error occurred",
            Code = "server_error_code"
          },
          Type = OriginalErrorCode.ServerError
        }
      };

      var ex = Assert.Throws<ServerException>(() => ErrorUtils.ParseAndRaiseError(originalErrorData, "Error reason", 500));
      Assert.Equal("Server error occurred", ex.Message);
      Assert.Equal(500, ex.Status);
    }

    [Fact]
    public void ParseAndRaiseError_RaisesValidationException_ForValidationError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Success = false,
        Error = new Error
        {
          Detail = new ErrorDetail
          {
            Message = "Validation error occurred",
            Code = "validation_error_code"
          },
          Type = OriginalErrorCode.ValidationError
        }
      };

      var ex = Assert.Throws<ValidationException>(() => ErrorUtils.ParseAndRaiseError(originalErrorData, "Error reason", 422));
      Assert.Equal("Validation error occurred", ex.Message);
      Assert.Equal(422, ex.Status);
    }

    [Fact]
    public void ParseAndRaiseError_RaisesDefaultClientException_WhenErrorTypeIsUnknown()
    {
      var unknownErrorData = new OriginalErrorData
      {
        Success = false,
        Error = new Error
        {
          Detail = new ErrorDetail
          {
            Message = "Unknown error occurred",
            Code = "unknown_error_code"
          },
          Type = "unknown_error_type"
        }
      };

      var ex = Assert.Throws<ClientException>(() => ErrorUtils.ParseAndRaiseError(unknownErrorData, "Error reason", 400));
      Assert.Equal("Unknown error occurred", ex.Message);
      Assert.Equal(400, ex.Status);
    }
  }
}
