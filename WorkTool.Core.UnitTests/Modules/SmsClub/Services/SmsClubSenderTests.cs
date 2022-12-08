namespace WorkTool.Core.UnitTests.Modules.SmsClub.Services;

public class SmsClubSenderTests : IDisposable
{
    private Mock<HttpMessageHandler>? httpMessageHandlerMock;
    private Mock<IDelay>? delayMock;

    private IReadOnlyDictionary<string, string>? endpoints;
    private SmsClubSender<object>? smsClubSender;
    private SendSmsClubRequest? successClubRequest;
    private SmsClubResponse? successClubResponse;
    private SendSmsClubRequest? faultClubRequest;
    private SmsClubResponse? faultClubResponse;
    private SmsClubSenderOptions? options;
    private int id;
    private string? faultPhoneNumber;
    private string? successPhoneNumber;
    private string? errorMessage;
    private string? sendUrl;
    private MessageItemsCollection<object> messageItemsCollection2;
    private MessageItemsCollection<object> messageItemsCollection15;

    [SetUp]
    public void Setup()
    {
        delayMock = new();
        httpMessageHandlerMock = new();
        errorMessage = "Данный номер находится в черном списке";
        id = 106;
        successPhoneNumber = "380989361131";
        faultPhoneNumber = "380989361130";

        successClubRequest = new SendSmsClubRequest()
        {
            PhoneNumbers = new[] { successPhoneNumber },
            Message = "test text",
            Recipient = "VashZakaz"
        };

        successClubResponse = new SmsClubResponse()
        {
            SuccessRequest = new SuccessRequest()
            {
                Info = new Dictionary<string, string>() { { id.ToString(), successPhoneNumber } }
            }
        };

        faultClubRequest = new SendSmsClubRequest()
        {
            PhoneNumbers = new[] { successPhoneNumber, faultPhoneNumber },
            Message = "test text",
            Recipient = "VashZakaz"
        };

        faultClubResponse = new SmsClubResponse()
        {
            SuccessRequest = new SuccessRequest()
            {
                Info = new Dictionary<string, string>() { { id.ToString(), successPhoneNumber } },
                AddInfo = new Dictionary<string, string>() { { faultPhoneNumber, errorMessage } }
            }
        };

        messageItemsCollection2 = CreateMessageItemsCollection<object>(2);
        messageItemsCollection15 = CreateMessageItemsCollection<object>(15);
        endpoints = Constants.DefaultEndpoints;
        sendUrl = $"{Constants.DefaultHost}{endpoints[Constants.SmsSendEndpointId]}";
        var httpClient = httpMessageHandlerMock
            .CreateClient()
            .SetBaseAddress(Constants.DefaultHost);
        options = SmsClubSenderOptions.Default;
        smsClubSender = new SmsClubSender<object>(httpClient, endpoints, options, delayMock.Object);
    }

    [Test]
    public async Task SendsSmsesAsync_Send2Sms_2ResponsesWithoutWait()
    {
        delayMock = delayMock.ThrowIfNull();
        smsClubSender = smsClubSender.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        var times = Times.Exactly(messageItemsCollection2.Count);
        SetupSuccess();

        await foreach (var massageItem in smsClubSender.SendsSmsesAsync(messageItemsCollection2))
        {
            SetupSuccess();
            massageItem.SuccessRequest
                .ThrowIfNull()
                .Info.Should()
                .HaveCount(1)
                .And.ContainKey(id.ToString())
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
        SetupSuccess();

        await foreach (var massageItem in smsClubSender.SendsSmsesAsync(messageItemsCollection15))
        {
            SetupSuccess();
            massageItem.SuccessRequest
                .ThrowIfNull()
                .Info.Should()
                .HaveCount(1)
                .And.ContainKey(id.ToString())
                .And.ContainValue(successPhoneNumber);
        }

        httpMessageHandlerMock.VerifyRequest(_ => true, httpMessageHandlerTimes);
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), delayMockTimes);
    }

    [Test]
    public async Task SendSmsAsync_SendSmsRequest_OkSuccess()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        successClubRequest = successClubRequest.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        SetupSuccess();
        var response = await smsClubSender.SendSmsAsync(successClubRequest);

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(id.ToString())
            .And.ContainValue(successPhoneNumber);

        httpMessageHandlerMock.VerifyRequest(_ => true, Times.Once());
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Test]
    public async Task SendSmsAsync_SendSmsRequest_OkFault()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        faultClubRequest = faultClubRequest.ThrowIfNull();
        successPhoneNumber = successPhoneNumber.ThrowIfNull();
        faultPhoneNumber = faultPhoneNumber.ThrowIfNull();
        errorMessage = errorMessage.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, sendUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(faultClubResponse));

        var response = await smsClubSender.SendSmsAsync(faultClubRequest);

        response.SuccessRequest
            .ThrowIfNull()
            .Info.Should()
            .HaveCount(1)
            .And.ContainKey(id.ToString())
            .And.ContainValue(successPhoneNumber);

        response.SuccessRequest
            .ThrowIfNull()
            .AddInfo.Should()
            .HaveCount(1)
            .And.ContainKey(faultPhoneNumber)
            .And.ContainValue(errorMessage);

        httpMessageHandlerMock.VerifyRequest(_ => true, Times.Once());
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Test]
    public async Task SendSmsAsync_ServerError_Exception()
    {
        smsClubSender = smsClubSender.ThrowIfNull();
        faultClubRequest = faultClubRequest.ThrowIfNull();
        delayMock = delayMock.ThrowIfNull();
        var times = Times.Exactly(HttpConsts.ErrorHttpStatusCodes.Length);

        foreach (var errorHttpStatusCode in HttpConsts.ErrorHttpStatusCodes.ToArray())
        {
            httpMessageHandlerMock
                .SetupRequest(HttpMethod.Post, sendUrl)
                .ReturnsResponse(errorHttpStatusCode);

            var func = () => smsClubSender.SendSmsAsync(faultClubRequest);

            await func.Should().ThrowAsync<HttpResponseException>();
        }

        httpMessageHandlerMock.VerifyRequest(_ => true, times);
        delayMock.Verify(x => x.DelayAsync(It.IsAny<TimeSpan>()), Times.Never());
    }

    public void Dispose()
    {
        httpMessageHandlerMock?.Object?.Dispose();
    }

    private void SetupSuccess()
    {
        httpMessageHandlerMock
            .SetupRequest(HttpMethod.Post, sendUrl)
            .ReturnsResponse(HttpStatusCode.OK, JsonContent.Create(successClubResponse));
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
