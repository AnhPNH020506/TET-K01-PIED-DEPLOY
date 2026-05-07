using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tet.Repository;
using Tet.Repository.Entity;

namespace Tet.Service.Order;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;
    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }
    public async Task<Response.CreateOrderResponse> CreateOrder(Request.CreateOrderRequest request)
    {
        var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        
        var userIdGuid = Guid.Parse(userId!);
        //List Object -> List Guid => Maping List thif dungf select
        var productIds = request.Prodcuts.Select(x => x.ProductId).Distinct().ToList();
        var query = _dbContext.Products.Where(x => productIds.Contains(x.Id));
        var productCount = await query.CountAsync();
        if (productCount != productIds.Count)
        {
            throw new Exception("Some Products are not identical");
        }
        var result = await query.ToListAsync();
        decimal totalAmount = 0;
        foreach (var prod in result)
        {
            var quantity = request.Prodcuts.First(x => x.ProductId == prod.Id).Quantity;
            if (quantity <= 0)
            {
                throw new Exception($"Quantity of product {prod.Id} must be greater than 0");
            }
            totalAmount += quantity * prod.Price;

        }
        if (totalAmount <= 0)
        {
            throw new Exception("Total amount must be greater than 0");}

        var order = new Repository.Entity.Order()
        {
            Id = Guid.NewGuid(),
            UserId = userIdGuid,
            TotalAmount = totalAmount,
            Address = request.Address,
            status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        List<OrderDetail> orderDetails = new List<OrderDetail>();
        foreach (var prod in result)
        {
            var quantity = request.Prodcuts.First(x => x.ProductId == prod.Id).Quantity;
            var orderDetail = new OrderDetail()
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = prod.Id,
                Quantity = quantity,
                UnitPrice = prod.Price,
                
            };
            orderDetails.Add(orderDetail);
        }
        if (orderDetails.Any())
        {
            _dbContext.AddRange(orderDetails);
            await _dbContext.SaveChangesAsync();
        }
        

        string description = $"Tet-{order.Id}";
        
        var response = new Response.CreateOrderResponse()
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            BankName = "MBBank",
            BankAccount = "VQRQAIDXC0464",
            Description = $"Payment for order {order.Id}",
            QRCode = "",
        };
        string qrCode = $"https://qr.sepay.vn/img?acc={response.BankAccount}" +
                        $"&bank={response.BankName}" +
                        $"&amount={(int)totalAmount}" +
                        $"&des={description}&" +
                        $"&template=qronly";
        response.QRCode = qrCode;
        return response;

    }

    public async Task SepayWebHokkHandler(Request.SepayWebhookRequest request)
    {
        var description = request.Code; //Handle trg hop mat dau gach ngang nha

        var raw = description.Replace("Tet", "");
        Guid? orderId = null;

        //var raw = description.Replace("IETPEE", "");

        if (raw.Length == 32)//mawcj dinh 1 guid co 32 ki tu
        //neu k du thi dong nghia id k hop le 
        
        //Vi orderId theo format la k co dau gach ngang
        
        {
            var formatted = $"{raw.Substring(0, 8)}-" +
                            $"{raw.Substring(8, 4)}-" +
                            $"{raw.Substring(12, 4)}-" +
                            $"{raw.Substring(16, 4)}-" +
                            $"{raw.Substring(20, 12)}";
            //Id dg theo dang string neen can chuyen doi sang kieeur Guid

            if (Guid.TryParse(formatted, out var guid))
            {
               orderId = guid;
            }
            
            //orderId = Guid.Parse(formatted)
        }
        else
        {
            throw new Exception("Invalid description format");
        }

        if (orderId == null)
        {
            throw new Exception("Invalid order id");
        }
        var query = _dbContext.Orders.Where(x => x.Id == orderId);
        var order = await query.Include(order => order.OrderDetails).FirstOrDefaultAsync();
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        if (order.status != "Pending")//don hang da dc xu ly r ma
        {
            throw new Exception("Order already in progress");
        }

        if (order.TotalAmount != request.TransferAmount)
        {
            throw new Exception("Transfer amount must be equal to transfer amount");
        }
        order.status = "Completed";
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
        var productIds =  order.OrderDetails.Select(x => x.ProductId).ToList();
            //tìm sản phẩm chứa trong cart với các id sau productIds của UserId
            
        var queryProdCart = _dbContext.CartDetails.Where(x => x.Cart.UserId == order.UserId &&
                                                              productIds.Contains(x.ProductId));
        var removeCartDetails = await queryProdCart.ToListAsync();
          _dbContext.CartDetails.RemoveRange(removeCartDetails);
          await _dbContext.SaveChangesAsync();
        //tìm dc r thi xóa đi
        // _dbContext.RemoveRange();

        throw new Exception();
    }
}
//Ban chat SePay
//Sex laf 1 thg ngoi lawng nghe tat ca giao dich cua minh trong tkk
// Va minh cos ther lam 1 thu neu co giao dich nao chuyen den thif no goi 1 API
// callback
//K phai giao dich nao cung la giao dichj cuar he thong minh
    //Giao dich tra tien lai tuwf ban A -> call APi
    //Giao dich mua hang tu he thong Tet
    //Gioa dichj tra tien co tuc
    
    //Call Api cua ai thi tuy mn setup voi he thong cua minh, nhuwng owr day
        //a muons nos call api cua minh de thong bao la da ck thanh cong
    
    //K phai tat ca giao dich nao cung la giao dich cua he thongs minhf, v thi ddeer
        //phan bt giao dich cua minh, thi chung ta can lafm dau, daasu ansa rieng   