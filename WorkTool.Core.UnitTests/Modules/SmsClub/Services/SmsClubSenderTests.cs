namespace WorkTool.Core.UnitTests.Modules.SmsClub.Services;

public class SmsClubSenderTests : IDisposable
{
    private Mock<HttpMessageHandler>? httpMessageHandlerMock;
    private Mock<IDelay>? delayMock;

    private SmsSenderEndpointsOptions? endpointsOptions;
    private SmsClubSender<object>? smsClubSender;
    private SendSmsRequest? successSendSmsClubRequest;
    private SmsResponse<DictionarySuccessRequest>? successDictionarySmsResponse;
    private SmsResponse<ArraySuccessRequest>? successArraySmsResponse;
    private SmsResponse<ArraySuccessRequest>? emptyArraySmsResponse;
    private SendSmsRequest? faultSendSmsClubRequest;
    private SmsResponse<DictionarySuccessRequest>? faultDictionarySmsResponse;
    private SmsSenderOptions? options;
    private SmsResponse<ObjectSuccessRequest<Balance>>? balanceSmsResponse;
    private double money;
    private int smsId;
    private string? currency;
    private string? faultPhoneNumber;
    private string? successPhoneNumber;
    private string? faultMessage;
    private string? sendUrl;
    private string? getSmsStatusUrl;
    private string? getOriginatorsUrl;
    private string? getBalanceUrl;
    private string? originator;
    private MessageItemsCollection<object> messageItemsCollection2;
    private MessageItemsCollection<object> messageItemsCollection15;

    [SetUp]
    public void Setup()
    {
        money = 8111.1700;
        currency = "UAH";
        originator = "test";
        delayMock = new();
        httpMessageHandlerMock = new();
        faultMessage = "Данный номер находится в черном списке";
        smsId = 106;
        successPhoneNumber = "380989361131";
        faultPhoneNumber = "380989361130";

        var httpClient = httpMessageHandlerMock
            .CreateClient()
            .SetBaseAddress(SmsClubSender.DefaultHostUri);

        balanceSmsResponse = new()
        {
            SuccessRequest = new()
            {
                Object = new() { Currency = currency, Money = money }
            }
        };

        successArraySmsResponse = new SmsResponse<ArraySuccessRequest>
        {
            SuccessRequest = new ArraySuccessRequest { Info = new[] { originator } }
        };

        emptyArraySmsResponse = new SmsResponse<ArraySuccessRequest>
        {
            SuccessRequest = new ArraySuccessRequest { Info = Array.Empty<string>() }
        };

        successSendSmsClubRequest = new SendSmsRequest
        {
            PhoneNumbers = new[] { successPhoneNumber },
            Message = "test text",
            Recipient = "VashZakaz"
        };

        successDictionarySmsResponse = new SmsResponse<DictionarySuccessRequest>
        {
            SuccessRequest = new DictionarySuccessRequest
            {
                Info = new Dictionary<string, string> { { smsId.ToString(), successPhoneNumber } }
            }
        };

        faultSendSmsClubRequest = new SendSmsRequest
        {
            PhoneNumbers = new[] { successPhoneNumber, faultPhoneNumber },
            Message = "test text",
            Recipient = "VashZakaz"
        };

        faultDictionarySmsResponse = new SmsResponse<DictionarySuccessRequest>
        {
            SuccessRequest = new DictionarySuccessRequest
            {
                Info = new Dictionary<string, string> { { smsId.ToString(), successPhoneNumber } },
                AddInfo = new Dictionary<string, string> { { faultPhoneNumber, faultMessage } }
            }
        };

        messageItemsCollection2 = CreateMessageItemsCollection<object>(2);
        messageItemsCollection15 = CreateMessageItemsCollection<object>(15);
        var smsOriginatorEndpoint = SmsSenderEndpointsOptions.DefaultSmsOriginatorEndpoint;
        var smsStatusEndpoint = SmsSenderEndpointsOptions.DefaultSmsStatusEndpoint;
        var smsBalanceEndpoint = SmsSenderEndpointsOptions.DefaultSmsBalanceEndpoint;
        getBalanceUrl = $"{SmsClubSender.DefaultHostUri}{smsBalanceEndpoint}";
        getOriginatorsUrl = $"{SmsClubSender.DefaultHostUri}{smsOriginatorEndpoint}";
        sendUrl =
            $"{SmsClubSender.DefaultHostUri}{SmsSenderEndpointsOptions.DefaultSmsSendEndpoint}";
        getSmsStatusUrl = $"{SmsClubSender.DefaultHostUri}{smsStatusEndpoint}";
        options = SmsSenderOptions.Default;
        endpointsOptions = SmsSenderEndpointsOptions.Default;
        smsClubSender = new SmsClubSender<object>(
            httpClient,
            endpointsOptions,
            options,
            delayMock.Object
        );
    }

    public async Task GetSmsStatusAsync_SendNotExistIds_Exception()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getSmsStatusUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(emptyArraySmsResponse));
        var func = () => smsClubSender.GetSmsStatusAsync(new[] { smsId.ToString() });

        var exception = await func.Should().ThrowAsync<NotFoundSmsesException>();
        exception.WithMessage($"Not found sms with id {smsId}.");
    }

    [Test]
    public async Task GetBalanceAsync_SendRequest_Balance()
    {
        smsClubSender = smsClubSender.ThrowIfNull();

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getBalanceUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(balanceSmsResponse));

        var response = await smsClubSender.GetBalanceAsync();
        var successRequest = response.SuccessRequest.ThrowIfNull();
        var obj = successRequest.Object.ThrowIfNull();
        obj.Currency.Should().Be(currency);
        obj.Money.Should().Be(money);
    }

    [Test]
    public async Task GetOriginatorsAsync_SendRequest_ListOriginators()
    {
        smsClubSender = smsClubSender.ThrowIfNull();

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getOriginatorsUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(successArraySmsResponse));

        var response = await smsClubSender.GetOriginatorsAsync();

        response.SuccessRequest.ThrowIfNull().Info.Should().HaveCount(1).And.Contain(originator);
    }

    [Test]
    public async Task GetSmsStatusAsync_GetStatus1Sms_SuccessOneRequest()
    {
        faultSendSmsClubRequest = faultSendSmsClubRequest.ThrowIfNull();
        smsClubSender = smsClubSender.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getSmsStatusUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(successDictionarySmsResponse));

        var response = await smsClubSender.GetSmsStatusAsync(new[] { smsId.ToString() });

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(smsId.ToString())
            .And.ContainValue(successPhoneNumber);

        httpMessageHandlerMock.VerifyRequest(r => true, Times.Once());
    }

    [Test]
    public async Task GetSmsStatusAsync_GetStatus2Sms_SuccessOneRequest()
    {
        faultSendSmsClubRequest = faultSendSmsClubRequest.ThrowIfNull();
        smsClubSender = smsClubSender.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        var ids = new[] { smsId.ToString(), smsId.ToString() };

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getSmsStatusUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(successDictionarySmsResponse));

        var response = await smsClubSender.GetSmsStatusAsync(ids);

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(smsId.ToString())
            .And.ContainValue(successPhoneNumber);

        httpMessageHandlerMock.VerifyRequest(r => true, Times.Once());
    }

    [Test]
    public async Task SendsSmsesAsync_Send2Sms_2ResponsesWithoutWait()
    {
        delayMock = delayMock.ThrowIfNull();
        smsClubSender = smsClubSender.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        var times = Times.Exactly(messageItemsCollection2.Count);
        SetupSuccessSend();

        await foreach (var massageItem in smsClubSender.SendsSmsesAsync(messageItemsCollection2))
        {
            SetupSuccessSend();

            massageItem.SuccessRequest
                .ThrowIfNull()
                .Info.Should()
                .HaveCount(1)
                .And.ContainKey(smsId.ToString())
                .And.ContainValue(successPhoneNumber);
        }

        httpMessageHandlerMock.VerifyRequest(_ => true, times);
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Test]
    public async Task SendsSmsesAsync_Send15Sms_153ResponsesWithWait()
    {
        options = options.ThrowIfNull();
        smsClubSender = smsClubSender.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        var httpMessageHandlerTimes = Times.Exactly(messageItemsCollection15.Count);
        var delayMockTimes = Times.Exactly(messageItemsCollection15.Count / options.Count);
        SetupSuccessSend();

        await foreach (var massageItem in smsClubSender.SendsSmsesAsync(messageItemsCollection15))
        {
            SetupSuccessSend();

            massageItem.SuccessRequest
                .ThrowIfNull()
                .Info.Should()
                .HaveCount(1)
                .And.ContainKey(smsId.ToString())
                .And.ContainValue(successPhoneNumber);
        }

        httpMessageHandlerMock.VerifyRequest(_ => true, httpMessageHandlerTimes);
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), delayMockTimes);
    }

    [Test]
    public async Task SendSmsAsync_SendSmsRequest_OkSuccess()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        successSendSmsClubRequest = successSendSmsClubRequest.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        SetupSuccessSend();

        var response = await smsClubSender.SendSmsAsync(successSendSmsClubRequest);

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(smsId.ToString())
            .And.ContainValue(successPhoneNumber);

        httpMessageHandlerMock.VerifyRequest(_ => true, Times.Once());
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Test]
    public async Task SendSmsAsync_SendSmsRequest_OkFault()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        faultSendSmsClubRequest = faultSendSmsClubRequest.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        faultPhoneNumber = faultPhoneNumber.ThrowIfNull();
        faultMessage = faultMessage.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, sendUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(faultDictionarySmsResponse));

        var response = await smsClubSender.SendSmsAsync(faultSendSmsClubRequest);

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(smsId.ToString())
            .And.ContainValue(successPhoneNumber);

        response.SuccessRequest
            .ThrowIfNull()
            .AddInfo.Should()
            .HaveCount(1)
            .And.ContainKey(faultPhoneNumber)
            .And.ContainValue(faultMessage);

        httpMessageHandlerMock.VerifyRequest(_ => true, Times.Once());
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Test]
    public async Task SendSmsAsync_ServerError_Exception()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        faultSendSmsClubRequest = faultSendSmsClubRequest.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        var times = Times.Exactly(HttpConsts.ErrorHttpStatusCodes.Length * 4);
        var smsIds = new[] { smsId.ToString() };

        foreach (var errorHttpStatusCode in HttpConsts.ErrorHttpStatusCodes.ToArray())
        {
            var errorMessage = CreateErrorMessage(errorHttpStatusCode);
            SetupFaultEndpoints(errorHttpStatusCode);

            var sendSmsAsyncFunc = () => smsClubSender.SendSmsAsync(faultSendSmsClubRequest);
            var getOriginatorsAsyncFunc = () => smsClubSender.GetOriginatorsAsync();
            var getSmsStatusAsyncFunc = () => smsClubSender.GetSmsStatusAsync(smsIds);
            var getBalanceAsyncFunc = () => smsClubSender.GetBalanceAsync();

            var exception = await sendSmsAsyncFunc.Should().ThrowAsync<HttpResponseException>();
            exception.WithMessage(errorMessage);
            exception = await getOriginatorsAsyncFunc.Should().ThrowAsync<HttpResponseException>();
            exception.WithMessage(errorMessage);
            exception = await getSmsStatusAsyncFunc.Should().ThrowAsync<HttpResponseException>();
            exception.WithMessage(errorMessage);
            exception = await getBalanceAsyncFunc.Should().ThrowAsync<HttpResponseException>();
            exception.WithMessage(errorMessage);
        }

        httpMessageHandlerMock.VerifyRequest(_ => true, times);
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    public void Dispose()
    {
        httpMessageHandlerMock?.Object?.Dispose();
    }

    private string CreateErrorMessage(HttpStatusCode httpStatusCode)
    {
        var reasonPhrase = httpStatusCode.GetReasonPhrase();
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{httpStatusCode} 1.1");

        if (reasonPhrase is not null)
        {
            stringBuilder.Append($" {reasonPhrase}");
        }

        var errorMessage = stringBuilder.ToString();

        return errorMessage;
    }

    private void SetupFaultEndpoints(HttpStatusCode httpStatusCode)
    {
        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, sendUrl)
            .ReturnsResponse(httpStatusCode);

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getSmsStatusUrl)
            .ReturnsResponse(httpStatusCode);

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getOriginatorsUrl)
            .ReturnsResponse(httpStatusCode);

        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, getBalanceUrl)
            .ReturnsResponse(httpStatusCode);
    }

    private void SetupSuccessSend()
    {
        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, sendUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(successDictionarySmsResponse));
    }

    private MessageItemsCollection<TParameters> CreateMessageItemsCollection<TParameters>(int count)
        where TParameters : notnull, new()
    {
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        var messageItems = new List<MessageItem<TParameters>>();

        for (var i = 0; i < count; i++)
        {
            messageItems.Add(new(new("Test", "Test", successPhoneNumber), new()));
        }

        return new MessageItemsCollection<TParameters>(messageItems);
    }
}
