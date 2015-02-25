namespace MSBuildCustomTasks
{
    using Microsoft.Build.Utilities;
    using Microsoft.Build.Framework;

    public class AddTwoNumbers : Task
    {
        [Required]
        public double Number1 { get; set; }

        [Required]
        public double Number2 { get; set; }

        [Output]
        public double Result { get; set; }

        public override bool Execute()
        {
            Result = Number1 + Number2;

            Log.LogMessage(MessageImportance.High, "Add two numbers", null);

            return true;
        }
    }
}
