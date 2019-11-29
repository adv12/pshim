using System.Management.Automation;

namespace PShim
{
    public class RectangleAmountCmdlet : RectangleCmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public float[] Amount { get; set; } = { 1 };

        public float SingleAmount => Amount[0];
    }
}
