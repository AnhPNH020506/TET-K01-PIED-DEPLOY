namespace Tet.Service.Order;

public interface IService
{
    public Task<Response.CreateOrderResponse> CreateOrder(Request.CreateOrderRequest request);
        // '{"gateway":"BIDV","transactionDate":"2026-04-06 23:41:15",
        // "accountNumber":"8886369921","subAccount":"96247BENTRAN",
        // "code":"TCMPBf9c3895c14b94583bad78673263",
        // "content":"QR - TCMPBf9c3895c14b94583bad786732631b1ca",//TetOrderId(bij tuwj xoa dau gach nen can handle lai casi nay)
        // "transferType":"in",
        // "description":"BankAPINotify QR - TCMPBf9c3895c14b94583bad786732631b1ca",
        // "transferAmount":2500,
        // "referenceCode":"bc8af415-13e4-4bf9-8352-a8af59df5808","accumulated":0,"id":48628369}'

    public Task SepayWebHokkHandler(Request.SepayWebhookRequest request);
}