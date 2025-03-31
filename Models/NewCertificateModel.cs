namespace InsuranceCertificates.Models
{
    public class NewCertificateModel
    {
        public string CustomerName { get; set; }
        public DateTime CustomerDateOfBirth { get; set; }
        public string InsuredItem { get; set; }
        public decimal InsuredSum { get; set; }
    }
}
