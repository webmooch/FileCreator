using System;

namespace FileCreator.Models
{
    internal class ProgressChangedEventArgs : EventArgs
    {
        public int PercentComplete { get; set; }
    }
}
