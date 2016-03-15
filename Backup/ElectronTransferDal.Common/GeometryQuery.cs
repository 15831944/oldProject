namespace ElectronTransferDal.Common
{
    public abstract class GeometryQuery
    {
        const string GEOMETRY_FIELD = "G3E_GEOMETRY";
        const string GEOMETRY_FIELD_ALIAS = "GEOMETRY";
        public virtual string GeometryField
        {
            get { return GEOMETRY_FIELD; }
        }
        public virtual string GeometryFieldAlias
        {
            get { return GEOMETRY_FIELD_ALIAS; }
        }

        public abstract string GeometryQueryText { get; }
    
    }
}
