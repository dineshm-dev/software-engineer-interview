using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip.InstallmentsService
{
    public class InstallmentRule
    {
        public InstallmentType installmentType { get; set; }

        public decimal purchaseAmount { get; set; }

        public decimal interestRate { get; set; }

        public short numberOfInstallmentPayment { get; set; }

        public int numberOfIntervalDays { get; set; }

    }

    public enum InstallmentType { 
      EmiWithoutInterest,
      EmiWithInterest,
    }
}
