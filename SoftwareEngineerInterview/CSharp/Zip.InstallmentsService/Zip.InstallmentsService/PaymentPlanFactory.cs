using System.Linq;
using System;

namespace Zip.InstallmentsService
{
    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the Zip product definition.
    /// </summary>
    public class PaymentPlanFactory
    {
        IPaymentStrategy strategy;
        public  PaymentPlanFactory(IPaymentStrategy strategy)
        {
            this.strategy = strategy;   
        }

        public PaymentPlanFactory() : this(new PaymentWithoutInterest()) { }
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount)
        {
            PaymentPlan plan = this.strategy.GenerateInstallmentPlan(purchaseAmount);
            return plan;
        }

        /// <summary>
        /// Followed Factory Method pattern
        /// Passing all data in object so installment can be calulated based on dynamic values.
        /// Decoupled the logic in seperate modules.
        /// </summary>
        /// <param name="installmentRule"></param>
        /// <returns></returns>
        public PaymentPlan CreateCustomPaymentPlan(InstallmentRule installmentRule)
        {
            PaymentPlan plan = new PaymentPlan();
            switch (installmentRule.installmentType)
            {
                case InstallmentType.EmiWithoutInterest:
                    plan = new PaymentWithoutInterest().GenerateInstallmentPlan(installmentRule);
                    break;
                case InstallmentType.EmiWithInterest:
                    plan = new PaymentWithInterest().GenerateInstallmentPlan(installmentRule);
                    break;
            }
            return plan;
        }
    }
}
