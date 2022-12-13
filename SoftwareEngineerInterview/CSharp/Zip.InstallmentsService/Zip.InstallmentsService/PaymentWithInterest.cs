using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip.InstallmentsService
{
    public class PaymentWithInterest : IPaymentStrategy
    {
        public PaymentPlan GenerateInstallmentPlan(decimal purchaseAmount)
        {
            decimal totalAmountDue = purchaseAmount;
            short installmentCount = 4;
            int days = 14;
            decimal interest = 10;

            InstallmentRule rule = new InstallmentRule
            {
                purchaseAmount = purchaseAmount,
                numberOfIntervalDays = days,
                numberOfInstallmentPayment = installmentCount
            };
            ///Add logger 
            if (!ValidateInstallment(rule)) {
                return null;
            }

            return this.GeneratePlan(totalAmountDue, installmentCount, days, interest);

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

            return this.GeneratePlan(totalAmountDue, installmentCount, days, installmentRule.interestRate);
        }

        private PaymentPlan GeneratePlan(decimal totalAmountDue, short installmentCount, int days, decimal interest)
        {
           

            var pennies = (totalAmountDue * 100) % installmentCount;
            var monthlyPayment = Math.Floor(totalAmountDue / installmentCount * 100);

            List<Installment> lstInstallments = new List<Installment>();
            for (int installmentNumber = 0; installmentNumber < installmentCount; installmentNumber++)
            {

                decimal extraPenny = 0;

                ///Adding remainder in first installment
                if (pennies > 0)
                {
                    extraPenny = pennies / 100;
                    pennies = 0;
                }

                decimal amount = (monthlyPayment / 100) + extraPenny; 
                decimal includeInterestAmount = amount + ((amount * interest) / 100);
                DateTime dueDate = DateTime.Now.AddDays(((installmentNumber) * days)
                       + (installmentNumber > 0 ? 1 : 0));

                lstInstallments.Add(new Installment { Amount = includeInterestAmount, DueDate = dueDate, Id = Guid.NewGuid() });

            }

            /*We can aslo use below Linq afunctional appraoch. We can also use above simple approach as well*/
            //var installments = from installmentNumber in Enumerable.Range(1, installmentCount)
            //                   let extraPenny = pennies-- > 0 ? 1 : 0
            //                   let amount = (monthlyPayment + extraPenny) / 100
            //                   let includeInterestAmount = amount + ((amount * interest)/100)
            //                   let dueDate = DateTime.Now.AddDays(((installmentNumber - 1) * days)
            //                          + (installmentNumber > 1 ? 1 : 0))
            //                   select new Installment { Amount = includeInterestAmount, DueDate = dueDate, Id = Guid.NewGuid() };
            return new PaymentPlan
            {
                Id = Guid.NewGuid(),
                Installments = lstInstallments.ToArray(),
                PurchaseAmount = totalAmountDue
            };
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

        

        private bool isvalidInterest(decimal interestRate)
        {
            return interestRate > 0;
        }
    }
}
