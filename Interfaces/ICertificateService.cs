using InsuranceCertificates.Domain;
using InsuranceCertificates.Models;

namespace InsuranceCertificates.Interfaces
{
    public interface ICertificateService
    {
        Certificate CreateCertificate(NewCertificateModel certificateModel);
    }
}
