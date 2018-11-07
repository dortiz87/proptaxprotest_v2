using Braintree;
using System;
using System.Linq;
using proptaxprotest.Models;
using Microsoft.AspNetCore.Mvc;

namespace proptaxprotest.Controllers
{
    public class CheckoutsController : Controller
    {

        public IBraintreeConfiguration config = new BraintreeConfiguration();


        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT

                                                                                };
        [HttpGet]
        [Route("checkouts")]
        public ActionResult New()
        {
            var gateway = config.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;
          
            return View();
        }

        [HttpPost]
        [Route("checkouts")]
        public ActionResult Create()
        {
            var gateway = config.GetGateway();
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(Request.Form["amount"].ToString());
            }
            catch (FormatException e)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request.Form["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return RedirectToAction("Show", new { id = transaction.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("New");
            }

        }

        [Route("checkouts/{id}")]
        public ActionResult Show(String id)
        {
            var gateway = config.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
            };

            ViewBag.Transaction = transaction;
            return View();
        }


        [HttpPost]
        [Route("checkouts/caclulate")]
        public JsonResult Calculate(decimal InvoiceAmount)
        {
            var result = Calculator.ConvFeeAmount(InvoiceAmount);
            return Json(result.ToString("#.##"));    
       }


     
        [Route("checkouts/calculatetotal")]
        public JsonResult CalculateTotal(decimal InvoiceAmount, decimal ConvenienceFee)
        {

            var result = Calculator.TotalFeeCalc(InvoiceAmount, ConvenienceFee);
            return Json(result.ToString("#.##"));
        }

      
    }

   
}

