using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentGateway.Models;
using PaymentGateway.Models.Clients;
using PaymentGateway.Services;
using PaymentGateway.Tests.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Tests
{
    public class PaymentGatewayFunctionUnitTest
    {
        [Fact]
        public async Task SetPayment_ReturnsAOkObjectResult_WithAPaymentResponse()
        {
            // Arrange
            var request = TestFactory.CreateHttpRequest(TestFactory.GetTestPayment());
            var mockAcquiringBankService = new Mock<IAcquiringBankService>();
            mockAcquiringBankService.Setup(serv => serv.SetPayment(It.IsAny<Payment>())).Returns(TestFactory.GetTestPaymentResponse());
            var paymentGatewayService = new PaymentGatewayService(new PaymentGatewayClient(mockAcquiringBankService.Object, new NullLoggerFactory()));
            var paymentGatewayFunction = new PaymentGatewayFunction(paymentGatewayService, new NullLoggerFactory());

            // Act
            var response = await paymentGatewayFunction.SetPayment(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var model = Assert.IsAssignableFrom<PaymentResponse>(okResult.Value);
        }

        [Fact]
        public async Task GetPayment_ReturnsAOkObjectResult_WithAPaymentEntityMasked()
        {
            // Arrange
            var paymentId = Guid.NewGuid().ToString();
            var request = TestFactory.CreateHttpRequest();
            var mockAcquiringBankService = new Mock<IAcquiringBankService>();
            mockAcquiringBankService.Setup(serv => serv.GetPayment(It.IsAny<Guid>())).Returns(TestFactory.GetTestPaymentEntityResponse(paymentId));
            var paymentGatewayService = new PaymentGatewayService(new PaymentGatewayClient(mockAcquiringBankService.Object, new NullLoggerFactory()));
            var paymentGatewayFunction = new PaymentGatewayFunction(paymentGatewayService, new NullLoggerFactory());

            // Act
            var response = await paymentGatewayFunction.GetPayment(request, paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var model = Assert.IsAssignableFrom<PaymentEntityMasked>(okResult.Value);
            Assert.Equal(paymentId, model.Id.ToString());
            Assert.Equal("XXXXXXXXXXXX4567", model.CardNumber);
            Assert.Equal("XXXX", model.ExpiryYear);
            Assert.Equal("XX", model.ExpiryMonth);
            Assert.Equal("XXX", model.CVV);
        }
    }
}
