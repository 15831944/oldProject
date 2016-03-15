namespace ElectronTransferModel
{
    public enum EntityState2
    {
        None = 0, Copy,Custom
    }
    public enum EntityType
    {
        None = 0, Label,ZxLabel
    }
    public enum EntityState
    {
        None = 0, Insert, Update, Delete, 
        InsertUpdate, InsertDelete,
        Old_Old_Old, Old_Old_Del, Old_Old_Add, Old_Old_Nal,
        Old_Del_Old, Old_Del_Del, Old_Del_Add, Old_Del_Nal,
        Old_Add_Old, Old_Add_Del, Old_Add_Add, Old_Add_Nal,

        Old_Nal_Old, Old_Nal_Del, Old_Nal_Add, Old_Nal_Nal,
        Add_Old_Old, Add_Old_Del, Add_Old_Add, Add_Old_Nal,
        Add_Del_Old, Add_Del_Del, Add_Del_Add, Add_Del_Nal,
        Add_Add_Old, Add_Add_Del, Add_Add_Add, Add_Add_Nal,
        Add_Nal_Old, Add_Nal_Del, Add_Nal_Add, Add_Nal_Nal,
        Del_Nal_Nal
    }
}