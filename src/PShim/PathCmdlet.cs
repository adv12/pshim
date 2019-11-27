using System;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;

namespace PShim
{
    public class PathCmdlet : PSCmdlet
    {

        private string[] _paths;
        protected bool ShouldExpandWildcards { get; set; }

        [Parameter(
                    Position = 0,
                    Mandatory = true,
                    ParameterSetName = "Literal")
                ]
        [Alias("PSPath")]
        [ValidateNotNullOrEmpty]
        public string[] LiteralPath
        {
            get => _paths;
            set => _paths = value;
        }

        [Parameter(
            Position = 0,
            Mandatory = true,
            ParameterSetName = "Path",
            ValueFromRemainingArguments = true)
        ]
        [ValidateNotNullOrEmpty]
        public string[] Path
        {
            get => _paths;
            set
            {
                _paths = value;
                ShouldExpandWildcards = true;
            }
        }

        protected bool IsFileSystemPath(ProviderInfo provider, string path)
        {
            if (provider.ImplementingType != typeof(FileSystemProvider))
            {
                ArgumentException ex = new ArgumentException(path +
                    " does not resolve to a path on the FileSystem provider.");
                ErrorRecord error = new ErrorRecord(ex, "InvalidProvider",
                    ErrorCategory.InvalidArgument, path);
                WriteError(error);
                return false;
            }
            return true;
        }
    }
}
