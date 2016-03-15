using System.Linq;
using ElectronTransferFramework.TextProcess;

namespace ElectronTransferDal.XmlDal
{
    /// <summary>
    /// 非打印字符处理
    /// </summary>
    public static class UnPrinted
    {
        private delegate string TextHandler(string text);

        private static void EnumProcess(XmlDB db,TextHandler processHandler)
        {
            foreach (var table in db.Tables)
            {
                foreach (var entity in table.Entities)
                {
                    var type = entity.GetType();
                    foreach (var property in type.GetProperties().Where(o => o.PropertyType == typeof (string)))
                    {
                        var rawText = (string)property.GetValue(entity, null);
                        if (!string.IsNullOrEmpty(rawText))
                        {

                            property.SetValue(entity, processHandler(rawText), null);
                        }
                    }
                
                }
            }
        }
        /// <summary>
        /// 非打印字符编码
        /// </summary>
        /// <param name="db"></param>
        public static void EncodeUnprited(this XmlDB db)
        {
            EnumProcess(db, text => text.ReplaceLowOrderASCIICharacters());
        }
        /// <summary>
        /// 非打印字符解码
        /// </summary>
        /// <param name="db"></param>
        public static void DecodeUnprited(this XmlDB db)
        {
            EnumProcess(db, text => text.GetLowOrderASCIICharacters());
        }
    }
}
