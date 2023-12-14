using System.Collections.Generic;

namespace Capstone.Models
{
    public class Protein
    {
        public int ProteinId { get; set; }
        public string SequenceName { get; set; }
        public string ProteinSequence { get; set; }
        public string Description { get; set; }
        public int FormatType { get; set; }
        public string Sequence1 { get; set; }
        public string Sequence2 { get; set; }
        public string Sequence3 { get; set; }
        public int UserId { get; set; }
    }

    /// <summary>
    /// Model to accept a new Protein
    /// </summary>
    public class RegisterProtein
    {
        public string SequenceName { get; set; }
        public string ProteinSequence { get; set; }
        public string Description { get; set; }
        public int FormatType { get; set; }
        public int UserId { get; set; }
    }

    public class ProteinResponse
    {
        public int ProteinId { get; set; }
        public string SequenceName { get; set; }
        public string ProteinSequence { get; set; }
        public string Description { get; set; }
        public int FormatType { get; set; }
        public List<string> BlueSequence { get; set; } = null;
        public List<string> GreenSequence { get; set; } = null;
        public List<string> YellowSequence { get; set; } = null;
        public int UserId { get; set; }
    }
}
