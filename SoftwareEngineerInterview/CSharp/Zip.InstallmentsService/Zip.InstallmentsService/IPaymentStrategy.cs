using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip.InstallmentsService
{
    public interface IPaymentStrategy
    {
        PaymentPlan GenerateInstallmentPlan(decimal purchaseAmount);

        PaymentPlan GenerateInstallmentPlan(InstallmentRule installmentRule);

        bool ValidateInstallment(InstallmentRule rule);
    }
}
