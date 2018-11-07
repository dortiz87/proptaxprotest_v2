namespace proptaxprotest.Models
{
    public class CheckoutCalculation
    {


       
        public decimal InvoiceAmount { get; set; }


   
        public decimal ConvenienceFee

        {
            get { return Calculator.ConvFeeAmount(InvoiceAmount); }
        }

        public decimal TotalAmount
        {
            get { return Calculator.TotalFeeCalc(InvoiceAmount, ConvenienceFee); }
        }

    }

    public class Calculator
    {
        public static decimal ConvFeeAmount(decimal InvoiceAmount)
        {
            decimal ProcFee = .029m;
            decimal TransFee = .30m;
            //decimal InvoiceAmount = invoiceAmount;
            decimal ConvAmount = (InvoiceAmount * ProcFee) + TransFee;
            return ConvAmount;
        }

        public static decimal TotalFeeCalc(decimal InvoiceAmount, decimal ConvenienceFee)
        {
            decimal totalamt = InvoiceAmount + ConvenienceFee;
            return totalamt;

        }
    }
}



