namespace grey.foodtruck.api.Entities
{
    public class GenericReturn
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object ReturnValue { get; set; }
    }
}
