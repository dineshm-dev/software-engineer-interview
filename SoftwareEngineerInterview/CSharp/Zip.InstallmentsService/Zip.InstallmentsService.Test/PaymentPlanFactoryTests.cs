using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Zip.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();
            
            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
            Assert.True(paymentPlan.Installments.Length > 0);
            Assert.Equal(4, paymentPlan.Installments.Length);

            Installment installment1 = paymentPlan.Installments[0];
            Assert.Equal(30.87m, installment1.Amount);
            Assert.Equal(DateTime.Now.Date, installment1.DueDate.Date);

            Installment installment2 = paymentPlan.Installments[1];
            Assert.Equal(30.86m, installment2.Amount);
            Assert.Equal(DateTime.Now.AddDays(15).Date, installment2.DueDate.Date);

            Installment installment3 = paymentPlan.Installments[2];
            Assert.Equal(30.86m, installment3.Amount);
            Assert.Equal(DateTime.Now.AddDays(29).Date, installment3.DueDate.Date);


            Installment installment4 = paymentPlan.Installments[3];
            Assert.Equal(30.86m, installment4.Amount);
            Assert.Equal(DateTime.Now.AddDays(43).Date, installment4.DueDate.Date);
        }


        [Fact]
        public void WhenCreateCustomPaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            InstallmentRule rule = new InstallmentRule
            {
                installmentType = InstallmentType.EmiWithoutInterest,
                numberOfIntervalDays = 14,
                numberOfInstallmentPayment = 4,
                purchaseAmount = 123.45M

            };

            // Act
            var paymentPlan = paymentPlanFactory.CreateCustomPaymentPlan(rule);

            // Assert
            paymentPlan.ShouldNotBeNull();
            Assert.True(paymentPlan.Installments.Length > 0);
            Assert.Equal(4, paymentPlan.Installments.Length);

            Installment installment1 = paymentPlan.Installments[0];
            Assert.Equal(30.87m, installment1.Amount);

            Installment installment2 = paymentPlan.Installments[1];
            Assert.Equal(30.86m, installment2.Amount);

            Installment installment3 = paymentPlan.Installments[2];
            Assert.Equal(30.86m, installment3.Amount);


            Installment installment4 = paymentPlan.Installments[3];
            Assert.Equal(30.86m, installment4.Amount);

        }


        [Fact]
        public void WhenCreateCustomPaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlanWithInterest()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            InstallmentRule rule = new InstallmentRule
            {
                installmentType = InstallmentType.EmiWithInterest,
                numberOfIntervalDays = 14,
                numberOfInstallmentPayment = 4,
                purchaseAmount = 40,
                interestRate = 10

            };

            // Act
            var paymentPlan = paymentPlanFactory.CreateCustomPaymentPlan(rule);

            // Assert
            paymentPlan.ShouldNotBeNull();
            Assert.True(paymentPlan.Installments.Length > 0);
            Assert.Equal(4, paymentPlan.Installments.Length);

            Installment installment1 = paymentPlan.Installments[0];
            Assert.Equal(11, installment1.Amount);

            Installment installment2 = paymentPlan.Installments[1];
            Assert.Equal(11, installment2.Amount);

            Installment installment3 = paymentPlan.Installments[2];
            Assert.Equal(11, installment3.Amount);


            Installment installment4 = paymentPlan.Installments[3];
            Assert.Equal(11, installment4.Amount);

        }

        [Fact]
        public void WhenCreatePaymentPlanWithInvalidValidOrderAmount_ShouldReturnInvalidValidPaymentPlan()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(-100);

            // Assert
            paymentPlan.ShouldBeNull();
            

        }
    }
}
