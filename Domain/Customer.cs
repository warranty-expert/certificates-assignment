namespace InsuranceCertificates.Domain;

public class Customer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required DateTime DateOfBirth { get; set; }
}
