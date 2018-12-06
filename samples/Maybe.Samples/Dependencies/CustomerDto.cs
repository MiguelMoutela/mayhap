namespace Maybe.Samples.Dependencies
{
    public class CustomerDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal AccountBalance { get; set; }

        public override string ToString()
            => $"{nameof(CustomerDto)}{{{Id}, {Name}, {AccountBalance:C}}}";
    }
}