using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;
using TechFixBackend.Repository;

namespace TechFixBackend.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _productRepository;
        private readonly IVendorRepository _vendorRepository;

        public FeedbackService(IFeedbackRepository productRepository, IVendorRepository vendorRepository)
        {
            _productRepository = productRepository;
            _vendorRepository = vendorRepository;
        }

      
    }
}