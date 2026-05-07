namespace Tet.Service.Order;

public class Request
{
    public class CreateOrderRequest
    {
       public string Address { get; set; }
      
       public List<ProductOrderRequest> Prodcuts { get; set; }
       
    }
    public class ProductOrderRequest
        {
       public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        }
    public class SepayWebhookRequest
    {
        public string Gateway { get; set; }
        public string TransactionDate { get; set; }
        public string AccountNumber { get; set; }
        public string SubAccount { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string TransferType { get; set; }
        public string Description { get; set; }
        public decimal TransferAmount { get; set; }
        public string ReferenceCode { get; set; }
        public decimal Accumulated { get; set; }
        public long Id { get; set; }
    }

    //tao don hang(He thong cua minh muon dat hang phari chuyen khoan trc)
        //set up chuyen khoan thanh cong bang tien thiet de xac nhan down hang
        //Neu tao ra don hang ma k chuyen khoan lien don hang se bi huy sau 15p
    //huy down hang
}