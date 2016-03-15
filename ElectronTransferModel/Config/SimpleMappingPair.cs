namespace ElectronTransferModel.Config
{
    public class SimpleMappingPair 
    {
        public string ClassName { get; set; }
        public string TableName { get; set; }
        public string UpdateView { get; set; }
        public static SimpleMappingPair Empty = new SimpleMappingPair();
    }
}