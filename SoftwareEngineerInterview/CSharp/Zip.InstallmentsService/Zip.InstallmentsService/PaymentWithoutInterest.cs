using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip.InstallmentsService
{
    public class PaymentWithoutInterest : IPaymentStrategy
    {
        public PaymentPlan GenerateInstallmentPlan(decimal purchaseAmount)
        {
            decimal totalAmountDue = purchaseAmount;
            short installmentCount = 4;
            int days = 14;

            InstallmentRule rule = new InstallmentRule
            {
                purchaseAmount = purchaseAmount,
                numberOfIntervalDays = days,
                numberOfInstallmentPayment = installmentCount
            };
            ///Add logger 
            if (!ValidateInstallment(rule))
            {
                return null;
            }

            return this.GeneratePlan(totalAmountDue, installmentCount, days);

        }

        public PaymentPlan GenerateInstallmentPlan(InstallmentRule installmentRule)
        {
            decimal totalAmountDue = installmentRule.purchaseAmount;
            short installmentCount = installmentRule.numberOfInstallmentPayment;
            int days = installmentRule.numberOfIntervalDays;

            ///Add logger 
            if (!ValidateInstallment(installmentRule))
            {
                return null;
            }

            return this.GeneratePlan(totalAmountDue, installmentCount, days);
        }

        public bool ValidateInstallment(InstallmentRule rule)
        {
            decimal purchaseamount = rule.purchaseAmount;
            int numberofdays = rule.numberOfIntervalDays;
            short numberofInstallment = rule.numberOfInstallmentPayment;
            return isValidAmount(purchaseamount);
        }

        private bool isValidAmount(decimal amount)
        {
            return amount > 0;
        }

        private bool isValidInstallment(short installmentCount, int days)
        {
            return installmentCount <= days;
        }

        private PaymentPlan GeneratePlan(decimal totalAmountDue, short installmentCount, int days)
        {
            var pennies = (totalAmountDue * 100) % installmentCount;
            var monthlyPayment = Math.Floor(totalAmountDue / installmentCount * 100);



            var installments = from installmentNumber in Enumerable.Range(1, installmentCount)
                               let extraPenny = pennies-- > 0 ? 1 : 0
                               let amount = (monthlyPayment + extraPenny) / 100
                               let dueDate = DateTime.Now.AddDays(((installmentNumber - 1) * days)
                                      + (installmentNumber > 1 ? 1 : 0))
                               select new Installment { Amount = amount, DueDate = dueDate, Id = Guid.NewGuid() };
            return new PaymentPlan
            {
                Id = Guid.NewGuid(),
                Installments = installments.ToArray(),
                PurchaseAmount = totalAmountDue
            };
        }
    }
}
