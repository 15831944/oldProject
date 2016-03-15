using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;

namespace ElectronTransferBll
{
    public class DBCustomFinder:Singleton<DBCustomFinderImplment>
    {
    }

    public class DBCustomFinderImplment : DictionaryWithEvent<string, XmlDBManager>
    {
    }
}
