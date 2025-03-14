namespace OrderProcessingApp.Models.Enums;

public enum OrderStatus
{
    New,
    InWarehouse,
    InShipping,
    Returned,
    Error,
    Closed
}